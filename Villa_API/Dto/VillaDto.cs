﻿namespace Villa_API.Dto
{
    public class VillaDto
    {
        public int Id { get; set; }
        public string Nombre { get; set; }
        public DateTime FechaCreacion => DateTime.Now;
    }
}
