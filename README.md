# London Stock API - MVP Design

## Data Model

To achieve this task, i have created these 3 primary entities:

1. **Stock class**: Represents a stock available for trading and is identified by a unique ticker symbol.
   - Fields: Ticker Symbol (string), Current Price (decimal), List of Trades (collection of Trade entities).

2. **Trade class**: Represents an individual trade transaction.
   - Fields: Stock (reference to the Stock entity), Price (decimal), Number of Shares (decimal), Broker ID (string), Timestamp (datetime).

3. **Broker class**: Represents an authorized broker for trading on the exchange.
   - Fields: Broker ID (string), Name (string), Address (string), List of Trades (collection of Trade entities).

## API Endpoints

1. **Receive Trade Notification** (POST /trades)
   - Accepts trade data in JSON format.
   - Request: Ticker Symbol, Price, Number of Shares, Broker ID.
   - Creates a new trade entry and associates it with the relevant stock and broker.
   - Response: string

2. **Get Stock Information** (GET /stocks/{ticker})
   - Retrieves information about a specific stock.
   - Response: Ticker Symbol, Current Price, List of Recent Trades.

3. **Get All Stocks** (GET /stocks)
   - Request information about all available stocks.
   - Response: List of Ticker Symbols and Current Prices for all stocks.

4. **Get Stock List** (GET /stock-list)
   - Retrieves a list of ticker symbols for a specified range of stocks.
   - Request body: List of Ticker Symbols.
   - Response: List of Ticker Symbols and Current Prices for the specified stocks.

