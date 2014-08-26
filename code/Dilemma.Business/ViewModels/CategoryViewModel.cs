using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Dilemma.Business.ViewModels
{
    /// <summary>
    /// The category view model.
    /// </summary>
    public class CategoryViewModel
    {
        /// <summary>
        /// Gets or sets the category id.
        /// </summary>
        public int CategoryId { get; set;  }

        /// <summary>
        /// Gets or sets the category name.
        /// </summary>
        public string Name { get; set; }
    }
}
