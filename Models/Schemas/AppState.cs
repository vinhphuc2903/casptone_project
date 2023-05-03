using System;
using System.Net.NetworkInformation;

namespace CapstoneProject.Models.Schemas
{
    public class AppState
    {
        public static AppState Instance { get; protected set; } = new AppState();
        private string _requestId;
        private SystemSetting _setting;
        private string _domain;
        private string _mediaUrl;
        private string _project;
        public AppState()
        {
            _setting = new SystemSetting() { Cdn = "" };
            _requestId = "";
            _domain = "";
            _mediaUrl = "";
            _project = "";
        }
        public virtual string RequestId
        {
            get
            {
                lock (_requestId)
                {
                    return _requestId;
                }
            }
            set
            {
                lock (_requestId)
                {
                    _requestId = value;
                }
            }
        }
        public virtual string Project
        {
            get
            {
                lock (_project)
                {
                    return _project;
                }
            }
            set
            {
                lock (_project)
                {
                    _project = value;
                }
            }
        }
        public virtual string Domain
        {
            get
            {
                lock (_domain)
                {
                    return _domain;
                }
            }
            set
            {
                lock (_domain)
                {
                    _domain = value;
                }
            }
        }
        public virtual SystemSetting Setting
        {
            get
            {
                if (_setting == null)
                {
                    return null;
                }
                lock (_setting)
                {
                    return _setting;
                }
            }
            set
            {
                if (_setting == null)
                {
                    _setting = value;
                }
                else
                {
                    lock (_setting)
                    {
                        _setting = value;
                    }
                }
            }
        }
        public virtual string MediaUrl
        {
            get
            {
                lock (_mediaUrl)
                {
                    return _mediaUrl;
                }
            }
            set
            {
                lock (_mediaUrl)
                {
                    _mediaUrl = value;
                }
            }
        }
    }
    public class SystemSetting
    {
        public string Cdn { set; get; }
    }
}

