using System;
using System.Collections.Generic;
using System.Text;

namespace Web.DTO.Core
{
    public class ProcedimentoSubGrupoHierarquiaDTO
    {
        public string SubGrupo { get; set; }
        public List<ProcedimentoDTO> Procedimentos { get; set; }
    }
}
