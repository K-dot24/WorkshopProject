﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Terminal3WebAPI.Models
{
    public class ReviewModel
    {
        public String userID { get; set; } = "";
        public String storeID { get; set; } = "";
        public String productID { get; set; } = ""; 
        public String review { get; set; } = "";
    }
}
