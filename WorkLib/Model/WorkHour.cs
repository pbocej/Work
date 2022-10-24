using System;
using System.ComponentModel.DataAnnotations;
using WorkLib.Data;
using WorkLib.Repository;

namespace WorkLib.Model
{

    /// <summary>Work hour object.</summary>
    public partial class WorkHour : Entity<WorkHour>
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="WorkHour"/> class.
        /// </summary>
        public WorkHour()
        {
            Date = DateTime.Now;
            From = DateTime.Now;
            To = DateTime.Now;
        }
        /// <summary>
        /// Initializes a new instance of the <see cref="WorkHour"/> class.
        /// </summary>
        /// <param name="data">The data.</param>
        public WorkHour(object data) : base(data)
        { }
        /// <summary>Gets or sets the work hour identifier.</summary>
        /// <value>The work hour identifier.</value>
        [Key]
        [Display(Name = "Id")]
        public int WorkHourID { get; set; }
        /// <summary>Gets or sets the work day date.</summary>
        /// <value>The date.</value>
        [Required(ErrorMessage = "Date is required.")]
        [DisplayFormat(ApplyFormatInEditMode = true, DataFormatString = "dd.MM.yyyy")]
        public DateTime Date { get; set; }
        /// <summary>Gets or sets work begin time.</summary>
        /// <value>Work begin time.</value>
        [Required(ErrorMessage = "Behin time is required.")]
        [DisplayFormat(ApplyFormatInEditMode = true, DataFormatString = "H:mm")]
        public DateTime From { get; set; }
        /// <summary>Gets or sets work end time.</summary>
        /// <value>Work end time.</value>
        [DisplayFormat(ApplyFormatInEditMode = true, DataFormatString = "H:mm")]
        public DateTime? To { get; set; }
        /// <summary>Gets the work hours.</summary>
        /// <value>The work hours.</value>
        [DisplayFormat(ApplyFormatInEditMode = true, DataFormatString = "H:mm")]
        [Editable(false)]
        public DateTime? Hours 
        { 
            get
            {
                if (To.HasValue)
                {
                    var ret = new DateTime(Date.Year, Date.Month, Date.Day, To.Value.Hour, To.Value.Minute, 0);
                    ret = ret.Add(new TimeSpan(From.Hour * -1, From.Minute * -1, 0));
                    return ret;
                }
                return null;
            }
        }
        /// <summary>Gets or sets the project identifier.</summary>
        /// <value>The project identifier.</value>
        public int? ProjectId { get; set; }
        /// <summary>
        /// Gets or sets the name of the project.
        /// </summary>
        /// <value>
        /// The name of the project.
        /// </value>
        public string ProjectName { get; set; }
        /// <summary>Gets or sets the subject.</summary>
        /// <value>The subject.</value>
        [Required(ErrorMessage = "Subject is required.")]
        public string Subject { get; set; }
        /// <summary>Gets or sets the work description.</summary>
        /// <value>The description.</value>
        public string Description { get; set; }
        /// <summary>Gets or sets the user identifier.</summary>
        /// <value>The user identifier.</value>
        public int UserId { get; set; }

        /// <summary>
        /// Deletes the current object.
        /// </summary>
        /// <param name="context">The data context.</param>
        /// <exception cref="System.NotImplementedException"></exception>
        public override void Delete(DbContext context = null)
        {
            WorkRepository.DeleteWorkHour(this, context);
        }

        /// <summary>
        /// Saves current object to data.
        /// </summary>
        /// <param name="context">The data context.</param>
        /// <exception cref="System.NotImplementedException"></exception>
        public override int Save(DbContext context = null)
        {
            return WorkRepository.SaveWorkHour(this, context);
        }
    }
}
