﻿using Anex.Api.Dto.Abstract;

namespace Anex.Api.Dto
{
    public class LedgerTagDto : EntityDto
    {
        public string? Description { get; set; }
        public int? Number { get; set; }
    }
}