using System.Collections.ObjectModel;
using App.Models.Entities;
using Microsoft.EntityFrameworkCore;

namespace App.DB;

public class UserContext : DbContext
{
    public Collection<UserEntity> Users { get; set; }
}

public class VideoContext : DbContext
{
    public Collection<VideoEntity> Videos { get; set; }
}