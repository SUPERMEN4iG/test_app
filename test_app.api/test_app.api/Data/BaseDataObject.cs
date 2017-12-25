using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace test_app.api.Data
{
    public interface IBaseDataObject
    {
        object Id { get; set; }
        DateTime DateCreate { get; set; }
    }

    public interface IBaseTimestampsDataObject : IBaseDataObject
    {
        DateTime DateUpdate { get; set; }
    }

    public interface IBaseDataObject<T> : IBaseDataObject
    {
        new T Id { get; set; }
    }

    public interface IBaseTimestampsDataObject<T> : IBaseDataObject<T>, IBaseTimestampsDataObject
    {
    }

    /// <summary>
    /// Базовое представление объекта данных
    /// </summary>
    /// <typeparam name="T">Тип Primary Key</typeparam>
    public abstract class BaseDataObject<T> : IBaseDataObject<T>
    {
        [Key]
        [Column(Order = 0)]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public T Id { get; set; }
        object IBaseDataObject.Id
        {
            get { return this.Id; }
            set { Id = (T)value; }
        }

        private DateTime? _dateCreate;
        [DataType(DataType.DateTime)]
        public DateTime DateCreate
        {
            get => _dateCreate ?? DateTime.UtcNow;
            set => _dateCreate = value;
        }
    }

    public abstract class BaseTimestampsDataObject<T> : BaseDataObject<T>, IBaseTimestampsDataObject<T>
    {
        private DateTime? _dateUpdated;
        [DataType(DataType.DateTime)]
        public DateTime DateUpdate
        {
            get => _dateUpdated ?? DateTime.UtcNow;
            set => _dateUpdated = value;
        }
    }
}
