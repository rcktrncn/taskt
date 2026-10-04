using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Security;
using System.Text;
using System.Windows.Automation;
using System.Windows.Forms;
using System.Xml.Linq;
using System.Xml.XPath;
using taskt.Core.Native.Windows;

namespace taskt.Core.AutomationElement_UIElement
{
    public static class UIElementInspector
    {
        /// <summary>
        /// AutomationElement Property Value types
        /// </summary>
        public enum AutomationElementPropertyValueTypes
        {
            String,
            Bool,
            Int,
        }

        /// <summary>
        /// fill color UIElement inspect finished
        /// </summary>
        public static readonly Color InspectedElementColor = Color.Yellow;

        /// <summary>
        /// fill color UIElement Inspect process
        /// </summary>
        public static readonly Color InspectProcessColor = Color.Salmon;

        /// <summary>
        /// get window name and handle from UIElement
        /// </summary>
        /// <param name="targetElement"></param>
        /// <returns></returns>
        public static (string, IntPtr) GetWindowNameAndHandleFromUIElement(AutomationElement targetElement)
        {
            // already specify window
            if (targetElement.Current.ControlType == ControlType.Window)
            {
                return (targetElement.Current.Name, (IntPtr)targetElement.Current.NativeWindowHandle);
            }

            var walker = TreeWalker.RawViewWalker;
            try
            {
                var desktopHandle = WindowAPI.GetDesktopWindowHandle();

                var currentElement = targetElement;
                while (true)
                {
                    var tparent = walker.GetParent(currentElement);
                    if (tparent != null)
                    {
                        var myHandle = (IntPtr)tparent.Current.NativeWindowHandle;
                        if (myHandle == desktopHandle)
                        {
                            // tparent is Desktop, currentElement is window(?)
                            return (currentElement.Current.Name, (IntPtr)currentElement.Current.NativeWindowHandle);
                        }
                        else if (tparent.Current.ControlType == ControlType.Window)
                        {
                            // tparent is window
                            return (tparent.Current.Name, myHandle);
                        }
                        else
                        {
                            currentElement = tparent;
                        }
                    }
                    else
                    {
                        throw new Exception("Parent Element is null");
                    }
                }
            }
            catch
            {
                // try other method
                var windowNames = WindowAPI.GetAllWindowNames();
                if ((targetElement.Current.NativeWindowHandle != 0) && (windowNames.Contains(targetElement.Current.Name)))
                {
                    return (targetElement.Current.Name, (IntPtr)targetElement.Current.NativeWindowHandle);
                }

                try
                {
                    var parent = walker.GetParent(targetElement);
                    while ((parent.Current.NativeWindowHandle == 0) || (!windowNames.Contains(parent.Current.Name)))
                    {
                        parent = walker.GetParent(parent);
                    }
                    return (parent.Current.Name, (IntPtr)parent.Current.NativeWindowHandle);
                }
                catch
                {
                    throw new Exception("Fail Get Window Name and Window Handle from UIElement");
                }
            }
        }

        /// <summary>
        /// get ControlType text
        /// </summary>
        /// <param name="elem"></param>
        /// <returns></returns>
        public static string GetControlTypeText(AutomationElement elem)
        {
            // MEMO: UIA_AppBarControlTypeId is null, why?
            var fullName = elem.Current.ControlType?.ProgrammaticName ?? ".UNKNOWN";
            return fullName.Substring(fullName.LastIndexOf('.') + 1);
        }

        /// <summary>
        /// get parent UIElement
        /// </summary>
        /// <param name="targetElement"></param>
        /// <returns></returns>
        /// <exception cref="Exception"></exception>
        public static AutomationElement GetParentUIElement(AutomationElement targetElement)
        {
            var walker = TreeWalker.RawViewWalker;

            var parent = walker.GetParent(targetElement);
            if (parent != null)
            {
                return parent;
            }
            else
            {
                throw new Exception("Parent UIElement does not exists");
            }
        }

        /// <summary>
        /// get property value
        /// </summary>
        /// <param name="targetElement"></param>
        /// <param name="propName"></param>
        /// <param name="tp"></param>
        /// <returns></returns>
        public static string GetPropertyValueAsString(AutomationElement targetElement, AutomationProperty propName, AutomationElementPropertyValueTypes tp)
        {
            try
            {
                return targetElement.GetCurrentPropertyValue(propName).ToString();
            }
            catch
            {
                switch (tp)
                {
                    case AutomationElementPropertyValueTypes.String:
                        return string.Empty;

                    case AutomationElementPropertyValueTypes.Bool:
                        return "false";

                    case AutomationElementPropertyValueTypes.Int:
                        return "0";

                    default:
                        return string.Empty;
                }
            }
        }

        /// <summary>
        /// get Window UIElement from window name (equals only)
        /// </summary>
        /// <param name="windowName"></param>
        /// <returns></returns>
        public static AutomationElement GetWindowUIElementFromWindowName(string windowName)
        {
            var wins = WindowAPI.GetAllWindowNamesAndHandles();
            var whnd = IntPtr.Zero;
            foreach ((var h, var n) in wins)
            {
                if (n == windowName)
                {
                    whnd = h;
                    break;
                }
            }
            if (whnd == IntPtr.Zero)
            {
                return null;
            }
            else
            {
                return AutomationElement.FromHandle(whnd);
            }
        }

        /// <summary>
        /// create xml-tree and TreeNode from window name
        /// </summary>
        /// <param name="windowName"></param>
        /// <param name="engine"></param>
        /// <returns></returns>
        public static (TreeNode, XElement, Dictionary<string, AutomationElement>) CreateXMLTreeAndTreeNodeFromWindowName(string windowName)
        {
            //AutomationElement winRoot;
            //using (var winElem = new InnerScriptVariable(engine))
            //{
            //    // get target window UIElement
            //    var getWinElem = new UIAutomationGetWindowUIElementFromWindowNameCommand()
            //    {
            //        v_WindowName = windowName,
            //        v_Result = winElem.VariableName,
            //    };
            //    getWinElem.RunCommand(engine);

            //    winRoot = (AutomationElement)winElem.VariableValue;
            //}

            var winRoot = GetWindowUIElementFromWindowName(windowName);

            // wait time func
            var stopDateTime = DateTime.Now.AddSeconds(App.Taskt_Settings.ClientSettings.GUIInspectSearchTime);
            var waitFunc = new Func<bool>(() =>
            {
                return DateTime.Now > stopDateTime;
            });

            //// create XML tree
            //var searchXML = new UIAutomationUIElementActionAfterSearchUIElementByXPathFromWindowNameCommand()
            //{
            //    v_WindowName = windowName,
            //    v_MaxSiblings = App.Taskt_Settings.ClientSettings.GUIInspectMaxSiblings.ToString(),
            //    v_MaxDepth = App.Taskt_Settings.ClientSettings.GUIInspectMaxDepth.ToString(),
            //};
            //(windowXMLTree, uiElementHashTable) = searchXML.DeepCreateUIElementXMLCore(winRoot, waitFunc, engine);

            (var windowXMLTree, var uiElementHashTable) = DeepCreateUIElementXMLCore(winRoot, waitFunc, App.Taskt_Settings.ClientSettings.GUIInspectMaxSiblings, App.Taskt_Settings.ClientSettings.GUIInspectMaxDepth);

            // DGB
            //Console.WriteLine("## element list");
            //foreach(var item in hashTable)
            //{
            //    var key = item.Key;
            //    var v = item.Value;
            //    Console.WriteLine($"{item.Key}, {v.Current.Name}, {v.Current.GetHashCode()}");
            //}

            var tree = CreateTreeNodeFromUIElement(winRoot);
            CreateChildTreeNodeFromXMLTree(tree, windowXMLTree, uiElementHashTable);
            //return tree;
            return (tree, windowXMLTree, uiElementHashTable);
        }

        /// <summary>
        /// create child tree node From XML tree
        /// </summary>
        /// <param name="tree"></param>
        /// <param name="root"></param>
        public static void CreateChildTreeNodeFromXMLTree(TreeNode tree, XElement root, Dictionary<string, AutomationElement> uiElementHashTable)
        {
            foreach (var element in root.Elements())
            {
                var node = CreateTreeNodeFromUIElement(GetUIElementFromXMLTree(element, uiElementHashTable));
                tree.Nodes.Add(node);
                if (element.Elements().Count() > 0)
                {
                    CreateChildTreeNodeFromXMLTree(node, element, uiElementHashTable);
                }
            }
        }

        /// <summary>
        /// get UIElement from XML Tree
        /// </summary>
        /// <param name="elem"></param>
        /// <param name="uiElementHashTable"></param>
        /// <returns></returns>
        public static AutomationElement GetUIElementFromXMLTree(XElement elem, Dictionary<string, AutomationElement> uiElementHashTable)
        {
            var h = elem.Attribute("Hash").Value;
            if (uiElementHashTable.ContainsKey(h))
            {
                return uiElementHashTable[h];
            }
            else
            {
                throw new Exception($"Strange UIElement Specified. Hash '{h}'");
            }
        }

        /// <summary>
        /// create treeNode from UIElement
        /// </summary>
        /// <param name="targetElement"></param>
        /// <returns></returns>
        public static TreeNode CreateTreeNodeFromUIElement(AutomationElement targetElement)
        {
            string nameValue = GetPropertyValueAsString(targetElement, AutomationElement.NameProperty, AutomationElementPropertyValueTypes.String);
            string localTypeValue = GetPropertyValueAsString(targetElement, AutomationElement.LocalizedControlTypeProperty, AutomationElementPropertyValueTypes.String);

            var node = new TreeNode
            {
                Text = $"\"{nameValue}\" {localTypeValue}",
                Tag = targetElement
            };
            return node;
        }

        /// <summary>
        /// create XML Element from UIElement
        /// </summary>
        /// <param name="targetElement"></param>
        /// <param name="hash">specify hash value if dup</param>
        /// <returns></returns>
        public static XElement CreateXmlElement(AutomationElement targetElement, string hash = "")
        {
            var node = new XElement(GetControlTypeText(targetElement));

            var cur = targetElement.Current;

            node.SetAttributeValue("AcceleratorKey", cur.AcceleratorKey);
            node.SetAttributeValue("AccessKey", cur.AccessKey);
            node.SetAttributeValue("AutomationId", cur.AutomationId);
            node.SetAttributeValue("ClassName", cur.ClassName);
            node.SetAttributeValue("FrameworkId", cur.FrameworkId);
            node.SetAttributeValue("HasKeyboardFocus", cur.HasKeyboardFocus.ToString());
            node.SetAttributeValue("HelpText", cur.HelpText);
            node.SetAttributeValue("IsContentElement", cur.IsContentElement.ToString());
            node.SetAttributeValue("IsControlElement", cur.IsControlElement.ToString());
            node.SetAttributeValue("IsEnabled", cur.IsEnabled.ToString());
            node.SetAttributeValue("IsKeyboardFocusable", cur.IsKeyboardFocusable.ToString());
            node.SetAttributeValue("IsOffscreen", cur.IsOffscreen.ToString());
            node.SetAttributeValue("IsPassword", cur.IsPassword.ToString());
            node.SetAttributeValue("IsRequiredForForm", cur.IsRequiredForForm.ToString());
            node.SetAttributeValue("ItemStatus", cur.ItemStatus);
            node.SetAttributeValue("LocalizedControlType", cur.LocalizedControlType);

            node.SetAttributeValue("Name", GetPropertyValueAsString(targetElement, AutomationElement.NameProperty, AutomationElementPropertyValueTypes.String)); ;

            node.SetAttributeValue("NativeWindowHandle", cur.NativeWindowHandle.ToString());
            node.SetAttributeValue("ProcessId", cur.ProcessId.ToString());

            node.SetAttributeValue("Hash", (string.IsNullOrEmpty(hash) ? targetElement.GetHashCode().ToString() : hash));

            return node;
        }

        /// <summary>
        /// get check max siblings function
        /// </summary>
        /// <param name="command"></param>
        /// <param name="engine"></param>
        /// <returns>When Func returns true, max siblings</returns>
        public static Func<int, bool> GetMaxSiblingsFunc(int maxSiblings)
        {
            if (maxSiblings == 0)
            {
                return new Func<int, bool>((n) => false);
            }
            else
            {
                return new Func<int, bool>((n) => (n > maxSiblings));
            }
        }

        /// <summary>
        /// get check Max Depth func
        /// </summary>
        /// <param name="command"></param>
        /// <param name="engine"></param>
        /// <returns>when Func returns true, max depth</returns>
        public static Func<int, bool> GetMaxDepthFunc(int maxDepth)
        {
            if (maxDepth == 0)
            {
                return new Func<int, bool>((d) => false);
            }
            else
            {
                return new Func<int, bool>((d) => (d > maxDepth));
            }
        }

        /// <summary>
        /// deep create UIElement XML
        /// </summary>
        /// <param name="command"></param>
        /// <param name="targetElement"></param>
        /// <param name="timeFunc"></param>
        /// <param name="maxDepth"></param>
        /// <param name="maxSiblings"></param>
        /// <param name="engine"></param>
        /// <returns></returns>
        public static (XElement, Dictionary<string, AutomationElement>) DeepCreateUIElementXMLCore(AutomationElement targetElement, Func<bool> timeFunc, int maxSiblings, int maxDepth)
        {
            var siblingFunc = GetMaxSiblingsFunc(maxSiblings);
            var depthFunc = GetMaxDepthFunc(maxDepth);

            var rootXML = CreateXmlElement(targetElement);

            var hashDic = new Dictionary<string, AutomationElement>()
            {
                { rootXML.GetHashCode().ToString(), targetElement }
            };

            var walker = TreeWalker.RawViewWalker;
            DeepCreateUIElementXML_DepthFirst(rootXML, targetElement, hashDic, walker, 1, siblingFunc, depthFunc, timeFunc);

            return (rootXML, hashDic);
        }

        /// <summary>
        /// deep create UIElement XML depth first
        /// </summary>
        /// <param name="parentXMLNode"></param>
        /// <param name="rootElement"></param>
        /// <param name="elemsDic"></param>
        /// <param name="walker"></param>
        /// <param name="depth"></param>
        /// <param name="siblingFunc">when Func returns true, max siblings</param>
        /// <param name="depthFunc">when Func returns true, max depth</param>
        /// <param name="timeFunc">when Func returns true, time out</param>
        public static void DeepCreateUIElementXML_DepthFirst(XElement parentXMLNode, AutomationElement rootElement, Dictionary<string, AutomationElement> elemsDic,
                                TreeWalker walker, int depth, Func<int, bool> siblingFunc, Func<int, bool> depthFunc, Func<bool> timeFunc)
        {
            var targetElement = walker.GetFirstChild(rootElement);
            int sibCnt = 0;
            while (targetElement != null)
            {
                // check hash dup
                string hash = targetElement.GetHashCode().ToString();
                if (elemsDic.ContainsKey(hash))
                {
                    int i = 1;
                    while (elemsDic.ContainsKey($"{hash}-{i}"))
                    {
                        i++;
                    }
                    hash += $"-{i}";
                }

                var childNode = CreateXmlElement(targetElement, hash);
                parentXMLNode.Add(childNode);
                elemsDic.Add(hash, targetElement);

                sibCnt++;
                if (siblingFunc(sibCnt) || timeFunc())
                {
                    return;
                }

                if (walker.GetFirstChild(targetElement) != null)
                {
                    depth++;
                    if (depthFunc(depth))
                    {
                        return;
                    }
                    else
                    {
                        DeepCreateUIElementXML_DepthFirst(childNode, targetElement, elemsDic, walker, depth, siblingFunc, depthFunc, timeFunc);
                    }
                }
                if (timeFunc())
                {
                    return;
                }

                targetElement = walker.GetNextSibling(targetElement);
            }
        }

        /// <summary>
        /// get UIElement Hash from UIElement
        /// </summary>
        /// <param name="elem"></param>
        /// <param name="uiElementHashTable">
        /// <returns></returns>
        public static string GetHashFromUIElement(AutomationElement elem, Dictionary<string, AutomationElement> uiElementHashTable)
        {
            try
            {
                return uiElementHashTable.FirstOrDefault(item => (item.Value == elem)).Key;
            }
            catch
            {
                throw new Exception($"hashTable firstOrDefalult {elem.Current.Name} {elem.Current.LocalizedControlType}");
            }
        }

        /// <summary>
        /// get XElement from UIElement
        /// </summary>
        /// <param name="elem"></param>
        /// <param name="xmlTree"></param>
        /// <param name="uiElementHashTable"></param>
        /// <returns></returns>
        public static XElement GetXElementFromUIElement(AutomationElement elem, XElement xmlTree, Dictionary<string, AutomationElement> uiElementHashTable)
        {
            var searchPath = $"//{GetControlTypeText(elem)}[@Hash=\"{GetHashFromUIElement(elem, uiElementHashTable)}\"]";
            return xmlTree.XPathSelectElement(searchPath);
        }

        /// <summary>
        /// Get XPath from XMLTree Root
        /// </summary>
        /// <param name="xml"></param>
        /// <param name="elem"></param>
        /// <param name="xmlTree"></param>
        /// <param name="uiElementHashTable"></param>
        /// <param name="useNameAttribute"></param>
        /// <param name="useAutomationIdAttribute"></param>
        /// <returns></returns>
        public static string GetXPathFromXMLTreeRoot(AutomationElement elem, XElement xmlTree, Dictionary<string, AutomationElement> uiElementHashTable, bool useNameAttribute = true, bool useAutomationIdAttribute = false)
        {
            var trgElement = GetXElementFromUIElement(elem, xmlTree, uiElementHashTable);

            // not found
            if (trgElement == null)
            {
                return string.Empty;
            }

            var xpath = string.Empty;
            while (trgElement.Parent != null)
            {
                xpath = CreateXPathFromXMLTree(trgElement, useNameAttribute, useAutomationIdAttribute) + xpath;
                trgElement = trgElement.Parent;
            }

            return xpath;
        }

        /// <summary>
        /// get xpath from UIElement XMLTree
        /// </summary>
        /// <param name="trgElem"></param>
        /// <param name="curElem"></param>
        /// <param name="xmlTree"></param>
        /// <param name="uiElementHashTable"></param>
        /// <param name="useNameAttribute"></param>
        /// <param name="useAutomationIdAttribute"></param>
        /// <returns></returns>
        public static string GetXPathFromUIElement(AutomationElement trgElem, AutomationElement curElem, XElement xmlTree, Dictionary<string, AutomationElement> uiElementHashTable, bool useNameAttribute = true, bool useAutomationIdAttribute = false)
        {
            var trgElement = GetXElementFromUIElement(trgElem, xmlTree, uiElementHashTable);

            var curElement = GetXElementFromUIElement(curElem, xmlTree, uiElementHashTable);
            if (curElement == null)
            {
                // curElem is root-window-node ?
                if (xmlTree.Attribute("Hash").Value == curElem.GetHashCode().ToString())
                {
                    curElement = xmlTree;
                }
            }

            // no found
            if ((trgElement == null) || (curElement == null))
            {
                return string.Empty;
            }

            string xpath = string.Empty;
            while (trgElement.Parent != null)
            {
                xpath = CreateXPathFromXMLTree(trgElement, useNameAttribute, useAutomationIdAttribute) + xpath;
                trgElement = trgElement.Parent;

                if (trgElement == curElement)
                {
                    break;
                }
            }

            if (trgElement == curElement)
            {
                return $"/{xpath}";
            }
            else
            {
                return string.Empty;
            }
        }

        /// <summary>
        /// create XPath from XML Tree
        /// </summary>
        /// <param name="elemNode"></param>
        /// <param name="useNameAttribute"></param>
        /// <param name="useAutomationIdAttribute"></param>
        /// <returns></returns>
        /// <exception cref="Exception"></exception>
        public static string CreateXPathFromXMLTree(XElement elemNode, bool useNameAttribute = true, bool useAutomationIdAttribute = false)
        {
            var parentNode = elemNode.Parent;

            string elemType = elemNode.Name.ToString();
            string elemHash = elemNode.Attribute("Hash").Value;
            string xpath;

            // use AutomationId attribute
            if (useAutomationIdAttribute && (elemNode.Attribute("AutomationId").Value != string.Empty))
            {
                xpath = $"/{elemType}[@AutomationId=\"{SecurityElement.Escape(elemNode.Attribute("AutomationId").Value)}\"]";
                var idNode = parentNode.XPathSelectElement($".{xpath}");
                if (idNode != null)
                {
                    if (idNode.Attribute("Hash").Value == elemHash)
                    {
                        return xpath;
                    }
                }
            }

            // use Name attribute
            if (useNameAttribute && (elemNode.Attribute("Name").Value != string.Empty))
            {
                xpath = $"/{elemType}[@Name=\"{SecurityElement.Escape(elemNode.Attribute("Name").Value)}\"]";
                var nameNode = parentNode.XPathSelectElement($".{xpath}");
                if (nameNode != null)
                {
                    if (nameNode.Attribute("Hash").Value == elemHash)
                    {
                        return xpath;
                    }
                }
            }

            // tag-name & index XPath
            xpath = $"/{elemType}";
            var typeNodes = parentNode.XPathSelectElements($".{xpath}");
            int idx = 1;
            foreach (XElement nd in typeNodes)
            {
                if (nd.Attribute("Hash").Value == elemHash)
                {
                    return $"{xpath}[{idx}]";
                }
                idx++;
            }

            throw new Exception("Fail Create UIElement XPath");
        }

        /// <summary>
        /// get (Microsoft) Inspect Tool like result from UIElement
        /// </summary>
        /// <param name="elem"></param>
        /// <returns></returns>
        public static string GetInspectResultFromUIElement(AutomationElement elem)
        {
            //string res = "";
            var res = new StringBuilder();

            try
            {
                res.Append($"Name:\t\"{GetPropertyValueAsString(elem, AutomationElement.NameProperty, AutomationElementPropertyValueTypes.String)}\"\r\n");
                res.Append($"ControlType:\t{GetControlTypeText(elem)}\r\n");
                res.Append($"LocalizedControlType:\t\"{elem.Current.LocalizedControlType}\"\r\n");
                res.Append($"IsEnabled:\t{elem.Current.IsEnabled}\r\n");
                res.Append($"IsOffscreen:\t{elem.Current.IsOffscreen}\r\n");
                res.Append($"IsKeyboardFocusable:\t{elem.Current.IsKeyboardFocusable}\r\n");
                res.Append($"HasKeyboardFocusable:\t{elem.Current.HasKeyboardFocus}\r\n");
                res.Append($"AccessKey:\t\"{elem.Current.AccessKey}\"\r\n");
                res.Append($"ProcessId:\t{elem.Current.ProcessId}\r\n");
                res.Append($"AutomationId:\t\"{elem.Current.AutomationId}\"\r\n");
                res.Append($"FrameworkId:\t\"{elem.Current.FrameworkId}\"\r\n");
                res.Append($"ClassName:\t\"{elem.Current.ClassName}\"\r\n");
                res.Append($"IsContentElement:\t{elem.Current.IsContentElement}\r\n");
                res.Append($"IsPassword:\t{elem.Current.IsPassword}\r\n");

                res.Append($"AcceleratorKey:\t\"{elem.Current.AcceleratorKey}\"\r\n");
                res.Append($"HelpText:\t\"{elem.Current.HelpText}\"\r\n");
                res.Append($"IsControlElement:\t{elem.Current.IsControlElement}\r\n");
                res.Append($"IsRequiredForForm:\t{elem.Current.IsRequiredForForm}\r\n");
                res.Append($"ItemStatus:\t\"{elem.Current.ItemStatus}\"\r\n");
                res.Append($"ItemType:\t\"{elem.Current.ItemType}\"\r\n");
                res.Append($"NativeWindowHandle:\t{elem.Current.NativeWindowHandle}\r\n");

                res.Append($"IsDockPatternAvailableProperty:\t{(bool)elem.GetCurrentPropertyValue(AutomationElement.IsDockPatternAvailableProperty)}\r\n");
                res.Append($"IsExpandCollapsePatternAvailableProperty:\t{(bool)elem.GetCurrentPropertyValue(AutomationElement.IsExpandCollapsePatternAvailableProperty)}\r\n");
                res.Append($"IsGridPatternAvailableProperty:\t{(bool)elem.GetCurrentPropertyValue(AutomationElement.IsGridPatternAvailableProperty)}\r\n");
                res.Append($"IsGridItemPatternAvailableProperty:\t{(bool)elem.GetCurrentPropertyValue(AutomationElement.IsGridItemPatternAvailableProperty)}\r\n");
                res.Append($"IsInvokePatternAvailableProperty:\t{(bool)elem.GetCurrentPropertyValue(AutomationElement.IsInvokePatternAvailableProperty)}\r\n");
                res.Append($"IsMultipleViewPatternAvailableProperty:\t{(bool)elem.GetCurrentPropertyValue(AutomationElement.IsMultipleViewPatternAvailableProperty)}\r\n");
                res.Append($"IsRangeValuePatternAvailableProperty:\t{(bool)elem.GetCurrentPropertyValue(AutomationElement.IsRangeValuePatternAvailableProperty)}\r\n");
                res.Append($"IsScrollPatternAvailableProperty:\t{(bool)elem.GetCurrentPropertyValue(AutomationElement.IsScrollPatternAvailableProperty)}\r\n");
                res.Append($"IsScrollItemPatternAvailableProperty:\t{(bool)elem.GetCurrentPropertyValue(AutomationElement.IsScrollItemPatternAvailableProperty)}\r\n");
                res.Append($"IsSelectionPatternAvailableProperty:\t{(bool)elem.GetCurrentPropertyValue(AutomationElement.IsSelectionPatternAvailableProperty)}\r\n");
                res.Append($"IsSelectionItemPatternAvailableProperty:\t{(bool)elem.GetCurrentPropertyValue(AutomationElement.IsSelectionItemPatternAvailableProperty)}\r\n");
                res.Append($"IsTablePatternAvailableProperty:\t{(bool)elem.GetCurrentPropertyValue(AutomationElement.IsTablePatternAvailableProperty)}\r\n");
                res.Append($"IsTableItemPatternAvailableProperty:\t{(bool)elem.GetCurrentPropertyValue(AutomationElement.IsTableItemPatternAvailableProperty)}\r\n");
                res.Append($"IsTextPatternAvailableProperty:\t{(bool)elem.GetCurrentPropertyValue(AutomationElement.IsTextPatternAvailableProperty)}\r\n");
                res.Append($"IsTogglePatternAvailableProperty:\t{(bool)elem.GetCurrentPropertyValue(AutomationElement.IsTogglePatternAvailableProperty)}\r\n");
                res.Append($"IsTransformPatternAvailableProperty:\t{(bool)elem.GetCurrentPropertyValue(AutomationElement.IsTransformPatternAvailableProperty)}\r\n");
                res.Append($"IsValuePatternAvailableProperty:\t{(bool)elem.GetCurrentPropertyValue(AutomationElement.IsValuePatternAvailableProperty)}\r\n");
                res.Append($"IsWindowPatternAvailableProperty:\t{(bool)elem.GetCurrentPropertyValue(AutomationElement.IsWindowPatternAvailableProperty)}\r\n");
            }
            catch (Exception ex)
            {
                res.Append($"Error: {ex.Message}");
            }

            return res.ToString();
        }

        /// <summary>
        /// highlight (wrap yellow line) UIElement
        /// </summary>
        /// <param name="elem"></param>
        public static void HighlightUIElement(AutomationElement elem, Color fillColor = default)
        {
            try
            {
                System.Windows.Rect rect = elem.Current.BoundingRectangle;
                Rectangle outerRect = new Rectangle
                {
                    X = (int)rect.X - 3,
                    Y = (int)rect.Y - 3,
                    Width = (int)rect.Width + 6,
                    Height = (int)rect.Height + 6
                };
                Rectangle middleRect = new Rectangle
                {
                    X = (int)rect.X - 2,
                    Y = (int)rect.Y - 2,
                    Width = (int)rect.Width + 4,
                    Height = (int)rect.Height + 4
                };
                Rectangle innerRect = new Rectangle
                {
                    X = (int)rect.X,
                    Y = (int)rect.Y,
                    Width = (int)rect.Width,
                    Height = (int)rect.Height
                };

                Graphics g = Graphics.FromHwnd(IntPtr.Zero);

                if (fillColor == default)
                {
                    fillColor = InspectedElementColor;
                }

                g.DrawRectangle(new Pen(Color.Black, 1), outerRect);
                g.DrawRectangle(new Pen(fillColor, 2), middleRect);
                g.DrawRectangle(new Pen(Color.Black, 1), innerRect);
            }
            catch
            {
                return;
            }
        }

        /// <summary>
        /// create xml-tree and TreeNode from Mouse cursor position
        /// </summary>
        /// <param name="mouseCursorPoint"></param>
        /// <returns>(windowName, TreeNode, XElement, UIElement-Hash)</returns>
        public static (string, TreeNode, XElement, Dictionary<string, AutomationElement>) CreateXMLTreeAndTreeNodeFromCursor(Point mouseCursorPoint)
        {
            var point = new System.Windows.Point(mouseCursorPoint.X, mouseCursorPoint.Y);
            var targetElement = AutomationElement.FromPoint(point);

            // get window name, handle
            (var winName, var whnd) = GetWindowNameAndHandleFromUIElement(targetElement);

            var thash = new Dictionary<string, AutomationElement>();

            var walker = TreeWalker.RawViewWalker;

            var targetChild = targetElement;
            var parentNode = walker.GetParent(targetElement);

            void AddChildrenXMLProcess(List<XElement> parentNodes, List<XElement> childrenNodes)
            {
                if (childrenNodes != null)
                {
                    var p = parentNodes[parentNodes.Count - 1];
                    foreach (var x in childrenNodes)
                    {
                        p.Add(x);
                    }
                }
            }

            List<XElement> childXMLs = null;
            while ((IntPtr)parentNode.Current.NativeWindowHandle != whnd)
            {
                // DBG
                //Console.WriteLine($"#Build Children {parentNode.Current.Name}, {EM_CanHandleUIElementExtentionMethods.GetControlTypeText(parentNode)}");

                var xmls = CreateChildUIElementXMLNodes(parentNode, targetChild, thash, walker);

                AddChildrenXMLProcess(xmls, childXMLs);

                childXMLs = xmls;

                targetChild = parentNode;

                parentNode = walker.GetParent(parentNode);

                // DGB
                //Console.WriteLine("Go ParentNode");
            }

            // now parentNode is Window
            var xxmls = CreateChildUIElementXMLNodes(parentNode, targetChild, thash, walker);
            AddChildrenXMLProcess(xxmls, childXMLs);
            childXMLs = xxmls;

            // create Window UIElement xml
            var windowElems = new List<XElement>();
            AddXMLNodeHashTableProcess(parentNode, windowElems, thash);
            AddChildrenXMLProcess(windowElems, childXMLs);

            // store global variables
            var windowXMLTree = windowElems[0];
            var uiElementHashTable = thash;

            var tree = CreateTreeNodeFromUIElement(parentNode);
            CreateChildTreeNodeFromXMLTree(tree, windowXMLTree, uiElementHashTable);

            //return tree;
            return (winName, tree, windowXMLTree, uiElementHashTable);
        }

        /// <summary>
        /// create child UIElements XML node
        /// </summary>
        /// <param name="parentNode"></param>
        /// <param name="targetChildNode"></param>
        /// <param name="myHashTable"></param>
        /// <param name="walker"></param>
        /// <returns></returns>
        private static List<XElement> CreateChildUIElementXMLNodes(AutomationElement parentNode, AutomationElement targetChildNode, Dictionary<string, AutomationElement> myHashTable, TreeWalker walker)
        {
            var elemList = new List<XElement>();

            var currentChild = walker.GetFirstChild(parentNode);
            while (currentChild != targetChildNode)
            {
                AddXMLNodeHashTableProcess(currentChild, elemList, myHashTable);
                currentChild = walker.GetNextSibling(currentChild);
            }
            // add target child UIElement node
            AddXMLNodeHashTableProcess(currentChild, elemList, myHashTable);

            return elemList;
        }

        /// <summary>
        /// create xml and add hash-table process
        /// </summary>
        /// <param name="targetElement"></param>
        /// <param name="parentXML"></param>
        /// <param name="myHashTable"></param>
        private static void AddXMLNodeHashTableProcess(AutomationElement targetElement, List<XElement> elems, Dictionary<string, AutomationElement> myHashTable)
        {
            var hash = targetElement.GetHashCode().ToString();
            if (myHashTable.ContainsKey(hash))
            {
                int i = 1;
                while (myHashTable.ContainsKey($"{hash}-{i}"))
                {
                    i++;
                }
                hash = $"{hash}-{i}";
            }
            myHashTable.Add(hash, targetElement);

            var txml = CreateXmlElement(targetElement, hash);
            elems.Add(txml);

            // DBG
            //Console.WriteLine($"UIElement added. {targetElement.Current.Name}, {EM_CanHandleUIElementExtentionMethods.GetControlTypeText(targetElement)}");
        }
    }
}
