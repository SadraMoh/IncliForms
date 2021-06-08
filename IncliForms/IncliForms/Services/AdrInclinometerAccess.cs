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
    public class AdrInclinometerAccess
    {
        static readonly Lazy<SQLiteAsyncConnection> lazyInitializer = new Lazy<SQLiteAsyncConnection>(() =>
        {
            return new SQLiteAsyncConnection(Constants.DatabasePath, Constants.Flags);
        });

        static SQLiteAsyncConnection db => lazyInitializer.Value;
        static bool initialized = false;

        public AdrInclinometerAccess()
        {
            InitializeAsync().SafeFireAndForget(false);
        }

        async Task InitializeAsync()
        {
            if (!initialized)
            {
                if (!db.TableMappings.Any(m => m.MappedType.Name == typeof(AdrInclinometer).Name))
                {
                    await db.CreateTablesAsync(CreateFlags.None, typeof(AdrInclinometer)).ConfigureAwait(false);
                }
                initialized = true;
            }
        }

        public Task<List<AdrInclinometer>> GetItemsAsync()
        {
            return db.Table<AdrInclinometer>().ToListAsync();
        }

        public Task<List<AdrInclinometer>> GetItemsNotDoneAsync()
        {
            // SQL queries are also possible
            return db.QueryAsync<AdrInclinometer>("SELECT * FROM [AdrInclinometer] WHERE [Done] = 0");
        }

        public Task<AdrInclinometer> GetItemAsync(int id)
        {
            return db.Table<AdrInclinometer>().Where(i => i.Id == id).FirstOrDefaultAsync();
        }

        public Task<int> SaveItemAsync(AdrInclinometer item)
        {
            if (item.Id != 0)
            {
                return db.UpdateAsync(item);
            }
            else
            {
                return db.InsertAsync(item);
            }
        }

        public async Task DeleteItemById(int id)
        {
            await db.QueryAsync<AdrInclinometer>($"DELETE FROM [AdrInclinometer] WHERE [Id] = {id}");
        }
    }
}
