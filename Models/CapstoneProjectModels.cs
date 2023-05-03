using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Runtime.CompilerServices;
using System.Threading.Tasks;
using CapstoneProject.Databases;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.DependencyInjection;

namespace CapstoneProject.Models
{
    public class CapstoneProjectModels
    {
        protected readonly IWebHostEnvironment _webHostEnvironment;

        private string _className = "";

        protected static int[] headerColorRgbArr = { 235, 241, 222 }; // red, green, blue

        protected readonly DataContext _context;

        public CapstoneProjectModels(IServiceProvider provider)
        {
            IWebHostEnvironment webHostEnvironment = provider.GetService<IWebHostEnvironment>();
             DataContext context = provider.GetService<DataContext>();
            _webHostEnvironment = webHostEnvironment ?? throw new ArgumentNullException(nameof(webHostEnvironment));
            _className = GetType().Name;
            _context = context ?? throw new ArgumentNullException(nameof(context));
        }

        protected static string GetActualAsyncMethodName([CallerMemberName] string name = null) => name;

    }
}