using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace web_app_asp_net_mvc_database_query.Models
{
    public class Lesson
    {
        /// <summary>
        /// Id
        /// </summary>
        [HiddenInput(DisplayValue = false)]
        public int Id { get; set; }

        /// <summary>
        /// Название урока
        /// </summary>
        [Required]
        [Display(Name = "Название урока", Order = 2)]
        public string LessonName { get; set; }

        /// <summary>
        /// Имя преподавателя
        /// </summary>
        [Required]
        [Display(Name = "Имя преподавателя", Order = 10)]
        public string Teacher { get; set; }

        /// <summary>
        /// Номер урока в расписании
        /// </summary>
        [Display(Name = "№", Order = 1)]
        public int SequentialNumber { get; set; }

        /// <summary>
        /// Группа, у которой проходит занятие
        /// </summary>
        [Required]
        [Display(Name = "Группа", Order = 30)]
        public string Group { get; set; }

    }
}