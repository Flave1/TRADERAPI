using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;
using TradeChat.Data.Documents;

namespace TradeChat.Services.Repository
{
    public interface IDocumentRepository<TDocument> where TDocument : IDocument
    {
        IQueryable<TDocument> AsQueryable();

        Task<IEnumerable<TDocument>> FilterByAsync(
            Expression<Func<TDocument, bool>> filterExpression);

        Task<TDocument> FindOneAsync(Expression<Func<TDocument, bool>> filterExpression);

        Task<TDocument> FindByIdAsync(string id);

        Task InsertOneAsync(TDocument document);

        Task InsertManyAsync(ICollection<TDocument> documents);

        Task ReplaceOneAsync(TDocument document);

        //Task UpdateOneAsync(TDocument document);

        //Task UpdateManyAsync(ICollection<TDocument> documents);

        Task DeleteOneAsync(Expression<Func<TDocument, bool>> filterExpression);

        Task DeleteByIdAsync(string id);

        Task DeleteManyAsync(Expression<Func<TDocument, bool>> filterExpression);
    }
}
