using IncliForms.Models;
using SQLite;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xamarin.Forms;

namespace IncliForms.Services
{
    public class SettingsAccess
    {
        public readonly Settings DefaultSettings = new Settings()
        {
            // ID ALWAYS 1
            Id = 0,
            OperatorName = "",
            SiteName = "",
        };

        static readonly Lazy<SQLiteAsyncConnection> lazyInitializer = new Lazy<SQLiteAsyncConnection>(() =>
        {
            return new SQLiteAsyncConnection(Constants.DatabasePath, Constants.Flags);
        });

        static SQLiteAsyncConnection db => lazyInitializer.Value;
        static bool initialized = false;

        public SettingsAccess()
        {
            InitializeAsync().SafeFireAndForget(false);
        }

        async Task InitializeAsync()
        {
            if (!initialized)
            {
                if (!db.TableMappings.Any(m => m.MappedType.Name == typeof(Settings).Name))
                {
                    await db.CreateTablesAsync(CreateFlags.None, typeof(Settings)).ConfigureAwait(false);
                }
                initialized = true;
            }
        }

        public Task<List<Settings>> GetItemsAsync()
        {
            return db.Table<Settings>().ToListAsync();
        }

        public async Task<Settings> GetItemAsync()
        {
            var res = (await db.QueryAsync<Settings>("SELECT * FROM [Settings] "));
            if (res.Count == 1) return res[0];

            int a = await SaveItemAsync(DefaultSettings);

            return DefaultSettings;
        }

        public Task<int> SaveItemAsync(Settings item)
        {
            if (item.Id == 0)
                return db.InsertAsync(item);
            else
                return db.UpdateAsync(item);
        }
    }
}
