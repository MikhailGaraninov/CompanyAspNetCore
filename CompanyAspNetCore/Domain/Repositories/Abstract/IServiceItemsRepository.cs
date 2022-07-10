using CompanyAspNetCore.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CompanyAspNetCore.Domain.Repositories.Abstract
{
    public interface IServiceItemsRepository
    {
        //Показать все услуги
        IQueryable<ServiceItem> GetServiceItems();
        //Выбрать услугу по идентификатору
        ServiceItem GetServiceItemById(Guid id);

        //Сохранить изменение услуги в базу данных
        void SaveServiceItem(ServiceItem entity);
        //Удалить услугу
        void DeleteServiceItem(Guid id);
    }
}