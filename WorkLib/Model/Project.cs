using System.ComponentModel.DataAnnotations;
using WorkLib.Data;
using WorkLib.Repository;

namespace WorkLib.Model
{

    /// <summary>Project type</summary>
    public partial class Project : Entity<Project>
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="Project"/> class.
        /// </summary>
        /// <param name="data">The data.</param>
        public Project(object data = null) : base(data)
        { }

        /// <summary>Gets or sets the project identifier.</summary>
        /// <value>The project identifier.</value>
        [Key]
        [Display(Name = "Id")]
        public int ProjectId { get; set; }
        /// <summary>Gets or sets the name of the project.</summary>
        /// <value>The name of the project.</value>
        [Required(ErrorMessage = "Project name is required.")]
        [Display(Name = "Name")]
        public string ProjectName { get; set; }
        /// <summary>Gets or sets the project description.</summary>
        /// <value>The project description.</value>
        [Display(Name = "Description")]
        public string ProjectDescription { get; set; }

        /// <summary>
        /// Deletes the current object.
        /// </summary>
        /// <param name="context">The data context.</param>
        /// <exception cref="System.NotImplementedException"></exception>
        public override void Delete(DbContext context = null)
        {
            WorkRepository.DeleteProject(this, context);
        }

        /// <summary>
        /// Saves current object to data.
        /// </summary>
        /// <param name="context">The data context.</param>
        /// <exception cref="System.NotImplementedException"></exception>
        public override int Save(DbContext context = null)
        {
            return WorkRepository.SaveProject(this, context).ProjectId;
        }
    }
}
