using Intsa.Models.Boards;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Linq;
using System.Threading.Tasks;

namespace Intsa.Models.Tests
{
    [TestClass]
    public class MyTestClass
    {
        [TestMethod]
        public async Task NoticeRepositoryAsyncAllMethodTest()
        {
            #region [0] DbContextOptions<T> Object Creation and ILoggerFactory Object Creation
            // db와의 테스트 할경우는 (identity 충돌하는 경우가 있기때문에) 인메모리로 우선 테스트 
            // [0] DbContextOptions<T> Object Creation and ILoggerFactory Object Creation
            var options = new DbContextOptionsBuilder<NoticeAppDbContext>()
                .UseInMemoryDatabase(databaseName: $"NoticeApp{Guid.NewGuid()}").Options;
            //.UseSqlServer("Data Source=(localdb)\\MSSQLLocalDB;Initial Catalog=NoticeApp;Integrated Security=True;Connect Timeout=60;Encrypt=False;TrustServerCertificate=False;ApplicationIntent=ReadWrite;MultiSubnetFailover=False").Options; 

            // ILoggerFactory object 
            var serviceProvider = new ServiceCollection().AddLogging().BuildServiceProvider();
            var factory = serviceProvider.GetService<ILoggerFactory>();
            #endregion

            #region [1] AddAsync() Method test 
            // [1] AddAsync() Method test 
            using (var context = new NoticeAppDbContext(options))
            {
                // [A] Arrange 
                var repository = new NoticeRepository(context, factory);
                var model = new BoardNotices { Name = "[1] 관리자", Title = "공지사항입니다.", Content = "내용입니다." };    // Id: 1

                // [B] Act 
                await repository.AddAsync(model);

            }
            using (var context = new NoticeAppDbContext(options))
            {
                // [C] Assert 
                Assert.AreEqual(1, await context.BoardNotices.CountAsync());     // 이 시점에 1개 일거다 주장 assert 
                var model = await context.BoardNotices.Where(n => n.Id == 1).SingleOrDefaultAsync();
                Assert.AreEqual("[1] 관리자", model.Name);
            }
            #endregion

            #region [2] GetAllAsync() Method test 
            // [2] GetAllAsync() Method test 
            using (var context = new NoticeAppDbContext(options))
            {
                // 트랜잭션 관련 코드는 InMemoryDatabase 공급자에서 지원안함 
                //using (var transaction = context.Database.BeginTransaction()) { transaction.Commit(); }
                // [A] Arrange 
                var repository = new NoticeRepository(context, factory);
                var model = new BoardNotices { Name = "[2] 홍길동", Title = "공지사항입니다 2", Content = "내용입니다 2" };      // Id: 2

                // [B] Act 
                await repository.AddAsync(model);
                await repository.AddAsync(new BoardNotices { Name = "[3] 백두산", Title = "공지사항입니다 3", Content = "내용입니다 3" });       // Id: 3
            }
            using (var context = new NoticeAppDbContext(options))
            {
                // [C] Assert 
                var repository = new NoticeRepository(context, factory);
                var models = await repository.GetAllAsync();
                Assert.AreEqual(3, models.Count()); 
            }

            #endregion

            #region [3] GetByIdAsync() Method test
            // [3] GetByIdAsync() Method test
            using (var context = new NoticeAppDbContext(options))
            {
                // Empty
            }
            using (var context = new NoticeAppDbContext(options))
            {
                var repository = new NoticeRepository(context, factory);
                var model = await repository.GetByIdAsync(2);
                Assert.IsTrue(model.Name.Contains("길동"));
                Assert.AreEqual("[2] 홍길동", model.Name);
            }

            #endregion

            #region [4] EditAsync() Method test 
            // [4] EditAsync() Method test 
            using (var context = new NoticeAppDbContext(options))
            {

            }

            using (var context = new NoticeAppDbContext(options))
            {
                var repository = new NoticeRepository(context, factory);
                var model = await repository.GetByIdAsync(2);

                model.Name = "[2] 임꺽정";
                await repository.EditAsync(model);

                var updateModel = await repository.GetByIdAsync(2);

                Assert.IsTrue(updateModel.Name.Contains("꺽정"));
                Assert.AreEqual("[2] 임꺽정", updateModel.Name);
                Assert.AreEqual("[2] 임꺽정",
                    (await context.BoardNotices.Where(m => m.Id == 2).SingleOrDefaultAsync())?.Name);

            }
            #endregion

            #region [5] DeleteAsync() Method test 
            // [5] DeleteAsync() Method test 
            using (var context = new NoticeAppDbContext(options))
            {

            }

            using (var context = new NoticeAppDbContext(options))
            {
                var repository = new NoticeRepository(context, factory);
                await repository.DeleteAsync(2);

                Assert.AreEqual(2, (await context.BoardNotices.CountAsync()));
                Assert.IsNull(await repository.GetByIdAsync(2)); 
            } 
            #endregion
            
            #region [6] GetAllAsync(PagingAsync) Method test 

            using (var context = new NoticeAppDbContext(options))
            {

            }

            using (var context = new NoticeAppDbContext(options))
            {
                int pageIndex = 0;
                int pageSize = 1; 

                var repository = new NoticeRepository(context, factory);
                var articleSet = await repository.GetAllAsync(pageIndex, pageSize);

                var firstName = articleSet.Records.FirstOrDefault()?.Name;
                var recordCount = articleSet.TotalRecords; 
                Assert.AreEqual("[3] 백두산", firstName);
                Assert.AreEqual(2, recordCount); 
            } 
            #endregion

            #region [7] GetStatus() Method test 
            //[7] GetStatus() Method test 
            using (var context = new NoticeAppDbContext(options))
            {
                int parentId = 1;

                var no1 = await context.BoardNotices.Where(m => m.Id == 1).SingleOrDefaultAsync();
                no1.ParentId = parentId;
                no1.IsPinned = true; // 공지글 설정 

                context.Entry(no1).State = EntityState.Modified;
                context.SaveChanges();

                var repository = new NoticeRepository(context, factory);
                var r = await repository.GetStatus(parentId);

                Assert.AreEqual(1, r.Item1);    // Pinned Count == 1
            } 
            #endregion
        }
    }
}
