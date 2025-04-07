﻿using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace PeerReviewApp.Models
{
    public class Document
    {
        public int Id { get; set; }
        public AppUser Uploader { get; set; }
        public string FilePath { get; set; }
    }
}
