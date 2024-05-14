﻿using System;
using System.Collections.Generic;

namespace CinemaAPI.Models;

public partial class MovieActor
{
    public int MovieActorId { get; set; }

    public int MovieId { get; set; }

    public int ActorId { get; set; }

    public string? ActorNickname { get; set; }

    public virtual Actor Actor { get; set; } = null!;

    public virtual Movie Movie { get; set; } = null!;
}
