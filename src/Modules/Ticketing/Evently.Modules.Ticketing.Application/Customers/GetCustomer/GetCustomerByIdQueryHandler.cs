using Dapper;
using Evently.Common.Application.Data;
using Evently.Common.Application.Messaging;
using Evently.Common.Domain;
using System.Data.Common;
using Evently.Modules.Ticketing.Domain.Customers;

namespace Evently.Modules.Ticketing.Application.Customers.GetCustomer;

internal sealed class GetCustomerByIdQueryHandler(IDbConnectionFactory dbConnectionFactory)
    : IQueryHandler<GetCustomerQuery, CustomerResponse>
{
    public async  Task<ResponseWrapper<CustomerResponse>> Handle(GetCustomerQuery request, CancellationToken cancellationToken)
    {
        await using DbConnection connection = await dbConnectionFactory.OpenConnectionAsync();

        const string sql =
            $"""
             SELECT
                 id AS {nameof(CustomerResponse.Id)},
                 email AS {nameof(CustomerResponse.Email)},
                 first_name AS {nameof(CustomerResponse.FirstName)},
                 last_name AS {nameof(CustomerResponse.LastName)}
             FROM ticketing.customers
             WHERE id = @CustomerId
             """;
        CustomerResponse? customer = await connection.QuerySingleOrDefaultAsync<CustomerResponse>(sql, request);

        if (customer is null)
        {
            return ResponseWrapper<CustomerResponse>.Fail(CustomerErrors.NotFound(request.CustomerId));
        }

        return ResponseWrapper<CustomerResponse>.Success(customer);
    }
}