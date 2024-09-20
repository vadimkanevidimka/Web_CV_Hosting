using MongoDB.Bson;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CVRecognizingService.Domain.Abstracts
{
    public interface IEntity
    {
        public ObjectId Id { get; set; }
    }
}
