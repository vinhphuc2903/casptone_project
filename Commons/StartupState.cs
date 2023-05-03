using System;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Configuration;

namespace CapstoneProject.Commons
{
    public class StartupState
    {
        public static StartupState Instance { get; protected set; } = new StartupState();
        private IWebHostEnvironment _webHostEnvironment;
        private IConfiguration _configuration;
        private IServiceProvider _services;

        protected internal StartupState()
        {
        }

        public virtual IServiceProvider Services
        {
            get
            {
                lock (_services)
                {
                    return _services;
                }
            }
            set
            {
                if (_services != null)
                {
                    throw new Exception("Can't set once a value has already been set.");
                }
                _services = value;
            }
        }

        public virtual HttpContext Current
        {
            get
            {
                lock (_services)
                {
                    if (_services == null)
                        return null;
                    IHttpContextAccessor httpContextAccessor = _services.GetService(typeof(IHttpContextAccessor)) as IHttpContextAccessor;
                    return httpContextAccessor?.HttpContext;
                }
            }
        }

        public virtual IWebHostEnvironment WebHostEnvironment
        {
            get
            {
                lock (_webHostEnvironment)
                {
                    return _webHostEnvironment;
                }
            }
            set
            {
                if (_webHostEnvironment != null)
                {
                    throw new Exception("Can't set once a value has already been set.");
                }
                _webHostEnvironment = value;
            }
        }

        public virtual IConfiguration Configuration
        {
            get
            {
                lock (_configuration)
                {
                    return _configuration;
                }
            }
            set
            {
                if (_configuration != null)
                {
                    throw new Exception("Can't set once a value has already been set.");
                }
                _configuration = value;
            }
        }

        public virtual string GetPathServer(string nameServer, string nameDefault = "~")
        {
            try
            {
                lock (_configuration)
                {
                    string path = _configuration.GetSection(nameServer).Value;
                    if (String.IsNullOrEmpty(path))
                    {
                        path = nameDefault;
                    }
                    return path;
                }
            }
            catch
            {
                return nameDefault;
            }
        }
    }
}
