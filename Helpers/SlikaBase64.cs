using System;

namespace Courses.Helpers
{
    public static class SlikaBase64
    {
        public static string Prikaz(byte[] slika)
        {
            return $"data:image/jpg;base64,{Convert.ToBase64String(slika)}";
        }
    }
}
