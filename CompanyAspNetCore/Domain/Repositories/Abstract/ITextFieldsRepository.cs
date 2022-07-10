using CompanyAspNetCore.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CompanyAspNetCore.Domain.Repositories.Abstract
{
    public interface ITextFieldsRepository
    {
        //Сделать выборку всех текстовых полей
        IQueryable<TextField> GetTextFields();
        //Выбрать текстовое поле по идентификатору
        TextField GetTextFieldById(Guid id);
        // Выбрать текстовое поле по ключевому слову
        TextField GetTextFieldByCodeWord(string codeword);

        //Сохранить изменение текстового поля в базу данных
        void SaveTextField(TextField entity);
        //Удалить текстовое поле
        void DeleteTextField(Guid id);
    }
}
