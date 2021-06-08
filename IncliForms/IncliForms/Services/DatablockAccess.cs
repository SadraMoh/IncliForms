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
    public class DatablockAccess
    {

        static readonly Lazy<SQLiteAsyncConnection> lazyInitializer = new Lazy<SQLiteAsyncConnection>(() =>
        {
            return new SQLiteAsyncConnection(Constants.DatabasePath, Constants.Flags);
        });

        static SQLiteAsyncConnection db => lazyInitializer.Value;
        static bool initialized = false;

        public DatablockAccess()
        {
            InitializeAsync().SafeFireAndForget(false);
        }

        async Task InitializeAsync()
        {
            if (!initialized)
            {
                if (!db.TableMappings.Any(m => m.MappedType.Name == typeof(AdrDatablock).Name))
                {
                    await db.CreateTablesAsync(CreateFlags.None, typeof(AdrDatablock)).ConfigureAwait(false);
                }
                initialized = true;
            }
        }

        public Task<List<AdrDatablock>> GetItemsAsync()
        {
            return db.Table<AdrDatablock>().ToListAsync();
        }

        public async Task<List<AdrDatablock>> GetItemAsync(int id)
        {
            var res = (await db.QueryAsync<AdrDatablock>($"SELECT * FROM [AdrDatablock] WHERE Id == {id}"));
            return res;
        }

        public async Task<List<AdrDatablock>> GetDatablocksByRecordId(int recordId)
        {
            var res = (await db.QueryAsync<AdrDatablock>($"SELECT * FROM [AdrDatablock] WHERE RecordId == {recordId}"));
            return res;
        }

        public Task<int> SaveItemAsync(AdrDatablock item)
        {
            if (item.Id == 0)
                return db.InsertAsync(item);
            else
                return db.UpdateAsync(item);
        }
    }
}
