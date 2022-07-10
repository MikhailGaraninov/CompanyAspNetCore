using CompanyAspNetCore.Domain.Entities;
using CompanyAspNetCore.Domain.Repositories.Abstract;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CompanyAspNetCore.Domain.Repositories.EntityFramework
{
    //Этот класс служит для связи обьектов сайта с базой данных
    public class EFTextFieldsRepository : ITextFieldsRepository
    {
        private readonly AppDbContext context;
        //Через внедрение зависимостей в конструкторе класса связываем обьект с параметром и реализуем методы(все члены интерфейса ITextFieldsRepository
        public EFTextFieldsRepository(AppDbContext context)
        {
            this.context = context;
        }

        public IQueryable<TextField> GetTextFields()
        {
            return context.TextFields; // Обращаемся к контексту и выбираем все записи из таблицы TextFields
        }

        public TextField GetTextFieldById(Guid id)
        {
            return context.TextFields.FirstOrDefault(x => x.Id == id);
        }
        // Выбрать текстовое поле по ключевому слову
        public TextField GetTextFieldByCodeWord(string codeWord)
        {
            return context.TextFields.FirstOrDefault(x => x.CodeWord == codeWord);
        }

        //Сохранить изменение текстового поля в базу данных
        public void SaveTextField(TextField entity)
        {
            if (entity.Id == default) //Если идентификатор равен значению по умолчанию, то значит создана новая запись и идннтификатора для нее еще нет
                context.Entry(entity).State = EntityState.Added;// Тогда для контекста отмечаем что это новый обьект ключом Added, Entity добавит обьект в базу данных
            else
                context.Entry(entity).State = EntityState.Modified;// если идентификатор уже есть у обьекта, помечаем флагом Modified обьект, это значит что этот обьект
         //   в базе данных есть, но у него изменились значения свойств(заголовок, описание, т.д.)
            context.SaveChanges();// Далее обращаемся к контексту и сохраняем изменения, в зависимости от флагом обьект будет либо добавлен либо изменен
        }
        //Удалить текстовое поле
        public void DeleteTextField(Guid id)
        {
            context.TextFields.Remove(new TextField() { Id = id });// обращаемся к таблице TextFields, вызываем метод Remove(внутри создается фейковый обьект с
            // нужным идентификатором и затем удаляется обьект с нужным идентификатором, для того чтобы не передавать весь обьект а только идентификатор id
            context.SaveChanges();
        }
    }
}
