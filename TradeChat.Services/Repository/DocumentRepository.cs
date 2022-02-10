using MongoDB.Driver;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;
using TradeChat.Data;
using TradeChat.Data.Documents;

namespace TradeChat.Services.Repository
{
    public class DocumentRepository<TDocument> : IDocumentRepository<TDocument> where TDocument : IDocument
    {
        private readonly IMongoCollection<TDocument> _collection;

        public DocumentRepository(IChatDatabaseSettings settings)
        {
            var database = new MongoClient(settings.ConnectionString).GetDatabase(settings.DatabaseName);
            _collection = database.GetCollection<TDocument>(GetCollectionName(typeof(TDocument)));
        }

        private protected string GetCollectionName(Type documentType)
        {
            return ((BsonCollectionAttribute)documentType.GetCustomAttributes(
                    typeof(BsonCollectionAttribute),
                    true)
                .FirstOrDefault())?.CollectionName;
        }

        public IQueryable<TDocument> AsQueryable()
        {
            return _collection.AsQueryable();
        }

        public virtual async Task<IEnumerable<TDocument>> FilterByAsync(
        Expression<Func<TDocument, bool>> filterExpression)
        {
            var documents = await _collection.FindAsync(filterExpression);
            return await documents.ToListAsync();
        }

        public virtual async Task<TDocument> FindOneAsync(Expression<Func<TDocument, bool>> filterExpression)
        {
            return await _collection.Find(filterExpression).FirstOrDefaultAsync();
        }

        public virtual async Task<TDocument> FindByIdAsync(string id)
        {
            var filter = Builders<TDocument>.Filter.Eq(doc => doc.Id, id);
            return await _collection.Find(filter).SingleOrDefaultAsync();
        }

        public virtual async Task InsertOneAsync(TDocument document)
        {
            await _collection.InsertOneAsync(document);
        }

        public virtual async Task InsertManyAsync(ICollection<TDocument> documents)
        {
            await _collection.InsertManyAsync(documents);
        }

        public virtual async Task ReplaceOneAsync(TDocument document)
        {
            var filter = Builders<TDocument>.Filter.Eq(doc => doc.Id, document.Id);
            await _collection.FindOneAndReplaceAsync(filter, document);
        }

        public void DeleteOne(Expression<Func<TDocument, bool>> filterExpression)
        {
            _collection.FindOneAndDelete(filterExpression);
        }

        public async Task DeleteOneAsync(Expression<Func<TDocument, bool>> filterExpression)
        {
            await _collection.FindOneAndDeleteAsync(filterExpression);
        }

        public async Task DeleteByIdAsync(string id)
        {
            var filter = Builders<TDocument>.Filter.Eq(doc => doc.Id, id);
            await _collection.FindOneAndDeleteAsync(filter);
        }

        public async Task DeleteManyAsync(Expression<Func<TDocument, bool>> filterExpression)
        {
            await _collection.DeleteManyAsync(filterExpression);
        }
    }
}
