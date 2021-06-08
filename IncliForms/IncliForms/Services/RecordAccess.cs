using IncliForms.Models;
using IncliForms.Models.Inclinometer;
using SQLite;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xamarin.Forms;

namespace IncliForms.Services
{
    public class RecordAccess
    {

        static readonly Lazy<SQLiteAsyncConnection> lazyInitializer = new Lazy<SQLiteAsyncConnection>(() =>
        {
            return new SQLiteAsyncConnection(Constants.DatabasePath, Constants.Flags);
        });

        static SQLiteAsyncConnection db => lazyInitializer.Value;
        static bool initialized = false;

        public RecordAccess()
        {
            InitializeAsync().SafeFireAndForget(false);
        }

        async Task InitializeAsync()
        {
            if (!initialized)
            {
                if (!db.TableMappings.Any(m => m.MappedType.Name == typeof(AdrRecord).Name))
                {
                    await db.CreateTablesAsync(CreateFlags.None, typeof(AdrRecord)).ConfigureAwait(false);
                }
                initialized = true;
            }
        }

        public Task<List<AdrRecord>> GetItemsAsync()
        {
            return db.Table<AdrRecord>().ToListAsync();
        }

        public async Task<List<AdrRecord>> GetItemAsync(int id)
        {
            var res = (await db.QueryAsync<AdrRecord>($"SELECT * FROM [AdrRecord] WHERE Id == {id}"));
            return res;
        }
        public async Task Clear()
        {
            await db.QueryAsync<AdrRecord>($"DELETE FROM [AdrRecord]");
        }

        public async Task DeleteById(int id)
        {
            await db.QueryAsync<AdrRecord>($"DELETE FROM [AdrRecord] WHERE [Id] == {id}");
        }

        public Task<int> SaveItemAsync(AdrRecord item)
        {
            if (item.Id == 0)
                return db.InsertAsync(item);
            else
                return db.UpdateAsync(item);
        }

        public Task<int> UpdateItem(AdrRecord item) => db.UpdateAsync(item);

    }
}
