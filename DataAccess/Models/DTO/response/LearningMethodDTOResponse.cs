﻿using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataAccess.Models.DTO.response
{
    public class LearningMethodDTOResponse
    {
        public int learning_method_id { get; set; }
        public string learning_method_name { get; set; }
    }
}
