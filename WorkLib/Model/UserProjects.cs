using System;
using WorkLib.Data;

namespace WorkLib.Model
{
    /// <summary>
    /// User project object
    /// </summary>
    public class UserProject : Project
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="UserProject"/> class.
        /// </summary>
        /// <param name="data">The data (Reader or Row).</param>
        public UserProject(object data = null) : base(data)
        { }
        /// <summary>
        /// Initializes a new instance of the <see cref="UserProject"/> class.
        /// </summary>
        /// <param name="project">The project objects.</param>
        public UserProject(Project project)
        {
            ProjectId = project.ProjectId;
            ProjectName = project.ProjectName;
            ProjectDescription = project.ProjectDescription;
        }

        /// <summary>
        /// Gets or sets the user identifier.
        /// </summary>
        /// <value>
        /// The user identifier.
        /// </value>
        public int UserId { get; set; }
        /// <summary>
        /// Gets or sets the project identifier.
        /// </summary>
        /// <value>
        /// The project identifier.
        /// </value>
        public bool Owned { get; set; }

        /// <summary>
        /// Deletes the current object.
        /// </summary>
        /// <param name="context">The data context.</param>
        /// <exception cref="System.NotImplementedException"></exception>
        public override void Delete(DbContext context = null)
        {
            throw new NotImplementedException();
        }

        /// <summary>
        /// Saves current object to data.
        /// </summary>
        /// <param name="context">The data context.</param>
        /// <exception cref="System.NotImplementedException"></exception>
        public override int Save(DbContext context = null)
        {
            throw new NotImplementedException();
        }
    }
}
