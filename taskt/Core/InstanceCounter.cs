using System;
using System.Collections.Generic;
using taskt.Core.Automation.Commands;
using static taskt.Core.Automation.Attributes.PropertyAttributes.PropertyInstanceType;

namespace taskt.Core
{
    // aliace for Instances Counter dictionary, too long!
    using InstanceCounterData = Dictionary<string, Dictionary<string, Dictionary<string, int>>>;

    public class InstanceCounter
    {
        //private SafeApplicationSettings appSettings = null;

        private  InstanceCounterData instances;

        // instance
        //private Dictionary<string, Dictionary<string, int>> databaseInstance = new Dictionary<string, Dictionary<string, int>>
        //{
        //    { "created", new Dictionary<string, int>() },
        //    { "used", new Dictionary<string, int>() }
        //};
        //private Dictionary<string, Dictionary<string, int>> excelInstance = new Dictionary<string, Dictionary<string, int>>
        //{
        //    { "created", new Dictionary<string, int>() },
        //    { "used", new Dictionary<string, int>() }
        //};
        //private Dictionary<string, Dictionary<string, int>> ieInstance = new Dictionary<string, Dictionary<string, int>>
        //{
        //    { "created", new Dictionary<string, int>() },
        //    { "used", new Dictionary<string, int>() }
        //};
        //private Dictionary<string, Dictionary<string, int>> webBrowserInstance = new Dictionary<string, Dictionary<string, int>>
        //{
        //    { "created", new Dictionary<string, int>() },
        //    { "used", new Dictionary<string, int>() }
        //};
        //private Dictionary<string, Dictionary<string, int>> stopWatchInstance = new Dictionary<string, Dictionary<string, int>>
        //{
        //    { "created", new Dictionary<string, int>() },
        //    { "used", new Dictionary<string, int>() }
        //};
        //private Dictionary<string, Dictionary<string, int>> wordInstance = new Dictionary<string, Dictionary<string, int>>
        //{
        //    { "created", new Dictionary<string, int>() },
        //    { "used", new Dictionary<string, int>() }
        //};
        //private Dictionary<string, Dictionary<string, int>> nlgInstance = new Dictionary<string, Dictionary<string, int>>
        //{
        //    { "created", new Dictionary<string, int>() },
        //    { "used", new Dictionary<string, int>() }
        //};
        //// variable type
        //private Dictionary<string, Dictionary<string, int>> dictionaryInstance = new Dictionary<string, Dictionary<string, int>>
        //{
        //    { "created", new Dictionary<string, int>() },
        //    { "used", new Dictionary<string, int>() }
        //};
        //private Dictionary<string, Dictionary<string, int>> dataTableInstance = new Dictionary<string, Dictionary<string, int>>
        //{
        //    { "created", new Dictionary<string, int>() },
        //    { "used", new Dictionary<string, int>() }
        //};
        //private Dictionary<string, Dictionary<string, int>> jsonInstance = new Dictionary<string, Dictionary<string, int>>
        //{
        //    { "created", new Dictionary<string, int>() },
        //    { "used", new Dictionary<string, int>() }
        //};
        //private Dictionary<string, Dictionary<string, int>> listInstance = new Dictionary<string, Dictionary<string, int>>
        //{
        //    { "created", new Dictionary<string, int>() },
        //    { "used", new Dictionary<string, int>() }
        //};
        //private Dictionary<string, Dictionary<string, int>> booleanInstance = new Dictionary<string, Dictionary<string, int>>
        //{
        //    { "created", new Dictionary<string, int>() },
        //    { "used", new Dictionary<string, int>() }
        //};
        //private Dictionary<string, Dictionary<string, int>> dateTimeInstance = new Dictionary<string, Dictionary<string, int>>
        //{
        //    { "created", new Dictionary<string, int>() },
        //    { "used", new Dictionary<string, int>() }
        //};
        //private Dictionary<string, Dictionary<string, int>> uiElementInstance = new Dictionary<string, Dictionary<string, int>>
        //{
        //    { "created", new Dictionary<string, int>() },
        //    { "used", new Dictionary<string, int>() }
        //};
        //private Dictionary<string, Dictionary<string, int>> colorInstance = new Dictionary<string, Dictionary<string, int>>
        //{
        //    { "created", new Dictionary<string, int>() },
        //    { "used", new Dictionary<string, int>() }
        //};
        //private Dictionary<string, Dictionary<string, int>> mailkitEMailInstance = new Dictionary<string, Dictionary<string, int>>
        //{
        //    { "created", new Dictionary<string, int>() },
        //    { "used", new Dictionary<string, int>() }
        //};
        //private Dictionary<string, Dictionary<string, int>> mailkitEMailListInstance = new Dictionary<string, Dictionary<string, int>>
        //{
        //    { "created", new Dictionary<string, int>() },
        //    { "used", new Dictionary<string, int>() }
        //};
        //private Dictionary<string, Dictionary<string, int>> webElementInstance = new Dictionary<string, Dictionary<string, int>>
        //{
        //    { "created", new Dictionary<string, int>() },
        //    { "used", new Dictionary<string, int>() }
        //};
        //private Dictionary<string, Dictionary<string, int>> windowHandleInstance = new Dictionary<string, Dictionary<string, int>>
        //{
        //    { "created", new Dictionary<string, int>() },
        //    { "used", new Dictionary<string, int>() }
        //};
        //private Dictionary<string, Dictionary<string, int>> numericInstance = new Dictionary<string, Dictionary<string, int>>
        //{
        //    { "created", new Dictionary<string, int>() },
        //    { "used", new Dictionary<string, int>() }
        //};

        public InstanceCounter()
        {
            //this.appSettings = settings;

            this.instances = new InstanceCounterData();
            var names = Enum.GetNames(typeof(InstanceType));
            foreach(var name in names)
            {
                instances.Add(name, new Dictionary<string, Dictionary<string, int>>
                {
                    { "created", new Dictionary<string, int>() },
                    { "used", new Dictionary<string, int>() },
                });
            }
        }

        public InstanceCounter(InstanceCounterData instances)
        {
            var names = Enum.GetNames(typeof(InstanceType));
            foreach(var name in names)
            {
                if (!instances.ContainsKey(name)) 
                {
                    throw new Exception($"Invalid Instances Counter Dictionary. Key '{name}' does not exists!");
                }
            }

            this.instances = instances;
        }

        /// <summary>
        /// add instance counter
        /// </summary>
        /// <param name="instanceName"></param>
        /// <param name="instanceType"></param>
        /// <param name="isUsed"></param>
        public void AddInstance(string instanceName, Automation.Attributes.PropertyAttributes.PropertyInstanceType instanceType, bool isUsed = false)
        {
            instanceName = FormatInstanceName(instanceName, instanceType);
            if (string.IsNullOrEmpty(instanceName))
            {
                return;
            }

            var targetDic = DecideTargetDictionary(instanceType, isUsed);

            if (targetDic.ContainsKey(instanceName))
            {
                targetDic[instanceName] = targetDic[instanceName] + 1;
            }
            else
            {
                targetDic.Add(instanceName, 1);
            }

            //Dictionary<string, int> targetDic = decideDictionary(instanceType.instanceType, isUsed);

            //if (string.IsNullOrEmpty(instanceName))
            //{
            //    instanceName = "";
            //}
            //instanceName = instanceName.Trim();
            //if (instanceName.Length == 0)
            //{
            //    return;
            //}

            ////if ((instanceType.autoWrapVariableMarker) && !(this.appSettings.EngineSettings.isWrappedVariableMarker(instanceName)))
            //if ((instanceType.autoWrapVariableMarker) &&
            //        !(VariableNameControls.IsWrappedVariableMarker(instanceName, appSettings)))
            //{
            //    //instanceName = this.appSettings.EngineSettings.wrapVariableMarker(instanceName);
            //    instanceName = VariableNameControls.GetWrappedVariableName(instanceName, appSettings);
            //}
            
            //if (targetDic.ContainsKey(instanceName))
            //{
            //    targetDic[instanceName] = targetDic[instanceName] + 1;
            //}
            //else
            //{
            //    targetDic.Add(instanceName, 1);
            //}
        }

        /// <summary>
        /// remove or reduce instance counter
        /// </summary>
        /// <param name="instanceName"></param>
        /// <param name="instanceType"></param>
        /// <param name="isUsed"></param>
        public void RemoveInstance(string instanceName, Automation.Attributes.PropertyAttributes.PropertyInstanceType instanceType, bool isUsed = false)
        {
            instanceName = FormatInstanceName(instanceName, instanceType);
            var targetDic = DecideTargetDictionary(instanceType, isUsed);

            if (targetDic.ContainsKey(instanceName))
            {
                if (targetDic[instanceName] > 1)
                {
                    targetDic[instanceName] = targetDic[instanceName] - 1;
                }
                else
                {
                    targetDic.Remove(instanceName);
                }
            }

            //Dictionary<string, int> targetDic = decideDictionary(instanceType.instanceType, isUsed);

            //if (string.IsNullOrEmpty(instanceName))
            //{
            //    instanceName = "";
            //}
            //instanceName = instanceName.Trim();
            //if (instanceName.Length == 0)
            //{
            //    return;
            //}

            ////if ((instanceType.autoWrapVariableMarker) && !(this.appSettings.EngineSettings.isWrappedVariableMarker(instanceName)))
            //if ((instanceType.autoWrapVariableMarker) && 
            //        !(VariableNameControls.IsWrappedVariableMarker(instanceName, appSettings)))
            //{
            //    //instanceName = this.appSettings.EngineSettings.wrapVariableMarker(instanceName);
            //    instanceName = VariableNameControls.GetWrappedVariableName(instanceName, appSettings);
            //}

            //if (targetDic.ContainsKey(instanceName))
            //{
            //    if (targetDic[instanceName] > 1)
            //    {
            //        targetDic[instanceName] = targetDic[instanceName] - 1;
            //    }
            //    else
            //    {
            //        targetDic.Remove(instanceName);
            //    }
            //}
        }

        /// <summary>
        /// get instance counter clone
        /// </summary>
        /// <param name="instanceType"></param>
        /// <param name="isUsed"></param>
        /// <returns></returns>
        public Dictionary<string, int> GetInstanceCounterClone(InstanceType instanceType, bool isUsed = false)
        {
            //Dictionary<string, int> targetDic = decideDictionary(instanceType, isUsed);

            //return new Dictionary<string, int>(targetDic);

            var instanceDic = this.instances[Enum.GetName(typeof(InstanceType), instanceType)];
            return (isUsed) ? instanceDic["used"] : instanceDic["created"];
        }

        /// <summary>
        /// get instances counter clone
        /// </summary>
        /// <returns></returns>
        public InstanceCounterData GetInstancesCounterClone()
        {
            return new InstanceCounterData(this.instances);
        }

        //private Dictionary<string, int> decideDictionary(InstanceType instanceType, bool isUsed = false)
        //{
        //    Dictionary<string, Dictionary<string, int>> targetDic;
        //    switch (instanceType)
        //    {
        //        case InstanceType.Boolean:
        //            targetDic = booleanInstance;
        //            break;
        //        case InstanceType.Color:
        //            targetDic = colorInstance;
        //            break;
        //        case InstanceType.DataBase:
        //            targetDic = databaseInstance;
        //            break;
        //        case InstanceType.DataTable:
        //            targetDic = dataTableInstance;
        //            break;
        //        case InstanceType.DateTime:
        //            targetDic = dateTimeInstance;
        //            break;
        //        case InstanceType.Dictionary:
        //            targetDic = dictionaryInstance;
        //            break;
        //        case InstanceType.Excel:
        //            targetDic = excelInstance;
        //            break;
        //        case InstanceType.IE:
        //            targetDic = ieInstance;
        //            break;
        //        case InstanceType.JSON:
        //            targetDic = jsonInstance;
        //            break;
        //        case InstanceType.List:
        //            targetDic = listInstance; ;
        //            break;
        //        case InstanceType.MailKitEMail:
        //            targetDic = mailkitEMailInstance;
        //            break;
        //        case InstanceType.MailKitEMailList:
        //            targetDic = mailkitEMailListInstance;
        //            break;
        //        case InstanceType.NLG:
        //            targetDic = nlgInstance;
        //            break;
        //        case InstanceType.Numeric:
        //            targetDic = numericInstance;
        //            break;
        //        case InstanceType.StopWatch:
        //            targetDic = stopWatchInstance;
        //            break;
        //        case InstanceType.UIElement:
        //            targetDic = uiElementInstance;
        //            break;
        //        case InstanceType.WebBrowser:
        //            targetDic = webBrowserInstance;
        //            break;
        //        case InstanceType.WebElement:
        //            targetDic = webElementInstance;
        //            break;
        //        case InstanceType.WindowHandle:
        //            targetDic = windowHandleInstance;
        //            break;
        //        case InstanceType.Word:
        //            targetDic = wordInstance;
        //            break;
        //        default:
        //            return null;
        //    }
        //    return (isUsed) ? targetDic["used"] : targetDic["created"];
        //}

        /// <summary>
        /// get instance type value by name
        /// </summary>
        /// <param name="instanceType"></param>
        /// <returns></returns>
        public static InstanceType GetInstanceType(string instanceType)
        {
            instanceType = instanceType.ToLower();
            var instanceTypes = Enum.GetValues(typeof(InstanceType));
            foreach(InstanceType v in instanceTypes)
            {
                var name = Enum.GetName(typeof(InstanceType), v);
                if (name.ToLower() == instanceType)
                {
                    return v;
                }
            }
            return InstanceType.none;

            //switch (instanceType.ToLower())
            //{
            //    case "boolean":
            //        return InstanceType.Boolean;
            //    case "color":
            //        return InstanceType.Color;
            //    case "database":
            //        return InstanceType.DataBase;
            //    case "datatable":
            //        return InstanceType.DataTable;
            //    case "datetime":
            //        return InstanceType.DateTime;
            //    case "dictionary":
            //        return InstanceType.Dictionary;
            //    case "excel":
            //        return InstanceType.Excel;
            //    case "ie":
            //        return InstanceType.IE;
            //    case "json":
            //        return InstanceType.JSON;
            //    case "list":
            //        return InstanceType.List;
            //    case "mailkitemail":
            //        return InstanceType.MailKitEMail;
            //    case "mailkitemaillist":
            //        return InstanceType.MailKitEMailList;
            //    case "numeric":
            //        return InstanceType.Numeric;
            //    case "stopwatch":
            //        return InstanceType.StopWatch;
            //    case "uielement":
            //        return InstanceType.UIElement;
            //    case "web browser":
            //        return InstanceType.WebBrowser;
            //    case "webelement":
            //        return InstanceType.WebElement;
            //    case "windowhandle":
            //        return InstanceType.WindowHandle;
            //    case "word":
            //        return InstanceType.Word;
            //    case "none":
            //    default:
            //        return InstanceType.none;
            //}
        }

        /// <summary>
        /// decide target instance counter dictionary
        /// </summary>
        /// <param name="instanceType"></param>
        /// <param name="isUsed"></param>
        /// <returns></returns>
        private Dictionary<string, int> DecideTargetDictionary(Automation.Attributes.PropertyAttributes.PropertyInstanceType instanceType, bool isUsed)
        {
            var typeDic = this.instances[Enum.GetName(typeof(InstanceType), instanceType.instanceType)];
            return (isUsed) ? typeDic["used"] : typeDic["created"];
        }

        /// <summary>
        /// format instance name
        /// </summary>
        /// <param name="name"></param>
        /// <returns></returns>
        private static string FormatInstanceName(string name, Automation.Attributes.PropertyAttributes.PropertyInstanceType instanceType)
        {
            if (string.IsNullOrEmpty(name))
            {
                name = "";
            }
            name = name.Trim();
            if (string.IsNullOrEmpty(name))
            {
                return "";
            }

            if ((instanceType.autoWrapVariableMarker) &&
                 !VariableNameControls.IsWrappedVariableMarker(name, App.Taskt_Settings))
            {
                name = VariableNameControls.GetWrappedVariableName(name, App.Taskt_Settings);
            }

            return name;
        }
    }
}
