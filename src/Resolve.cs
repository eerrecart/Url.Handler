using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Text;

namespace Url.Handler
{
    public class Resolve
    {
        private List<Filter> FiltersToApply;
        /// <summary>
        /// Stores the original URL
        /// </summary>
        private string Url { get; set; }
        /// <summary>
        /// Stores the modified url between filters executions
        /// </summary>
        private string OuputUrl { get; set; }
        public bool IsRelative { get; set; }
        /// <summary>
        /// Event that occurs before all the process begins
        /// </summary>
        public event FilterHandler BeforeStart;
        /// <summary>
        /// Event that occurs before every filter is executed
        /// </summary>
        public event FilterHandler BeforeFilterApply;
        /// <summary>
        /// Event that occurs after every filter is executed, the base class executes a validation input string to check if is relative or absolute url
        /// </summary>
        public event FilterHandler AfterFilterApply;

        public Resolve(string url)
        {
            Url = url;
            FiltersToApply = new List<Filter>();
            Validations(Url);

            BeforeStart         += Resolve_BeforeStart;
            AfterFilterApply    += Resolve_AfterFilterApply;
            
        }

        void Resolve_BeforeStart(Filter currentFilter, Resolve holder)
        {
            holder.Validations(holder.Url);
        }

        void Resolve_AfterFilterApply(Filter currentFilter, Resolve holder)
        {
            holder.Validations(holder.OuputUrl);
        }

        private void Validations(string url)
        {
            if (string.IsNullOrWhiteSpace(url))
                throw new ConfigurationException("The url must be a valid URL");

            try
            {
                IsRelative = !new Uri(url).IsAbsoluteUri;
            }
            catch
            {
                IsRelative = (new Uri(new Uri("http://nohost/"), url) != null);
            }
        }

        /// <summary>
        /// Add a filter to modify the original URL
        /// </summary>
        /// <param name="filter"></param>
        /// <returns></returns>
        public bool AddFilter(Filter filter)
        {
            if (filter != null && !FiltersToApply.Contains(filter))
            {
                FiltersToApply.Add(filter);
                return true;
            }
            else
                return false;
        }

        /// <summary>
        /// Executes al aviable filters, and returns the resultant URL.
        /// </summary>
        /// <returns></returns>
        public string ApplyFilters()
        {
            if (FiltersToApply.Any())
            {
                int ix_filter = 1;
                try
                {
                    
                    var start = FiltersToApply.First();

                    BeforeStart(start, this);
                    BeforeFilterApply(start, this);

                    OuputUrl = start(Url, this);

                    AfterFilterApply(start, this);

                    foreach (Filter f in FiltersToApply.Skip(1))
                    {
                        BeforeFilterApply(f, this);
                        OuputUrl = f(OuputUrl, this);
                        AfterFilterApply(f, this);

                        ix_filter++;
                    }
                }
                catch (Exception ex)
                {
                    throw new ApplicationException(string.Format("Error occurred while applying filter # {0}, details: {1}", ix_filter, ex.Message));
                }
                
                return OuputUrl;
            }
            else
                return Url;
            
        }
    }

    /// <summary>
    /// Creates filters to add an execute over an sepecific url.
    /// </summary>
    /// <param name="urlToFilter"></param>
    /// <returns></returns>
    public delegate string Filter(string urlToFilter, Resolve holder);

    // The delegate procedure we are assigning to our object
    public delegate void FilterHandler(Filter currentFilter, Resolve myArgs);

    
}
