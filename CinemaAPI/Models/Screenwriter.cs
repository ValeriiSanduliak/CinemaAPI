﻿using System;
using System.Collections.Generic;

namespace CinemaAPI.Models;

public partial class Screenwriter
{
    public int ScreenwriterId { get; set; }

    public string ScreenwriterFullName { get; set; } = null!;

    public virtual ICollection<MovieScreenwriter> MovieScreenwriters { get; set; } = new List<MovieScreenwriter>();
}
