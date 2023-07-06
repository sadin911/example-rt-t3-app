# pygetstatus

`pygetstatus.py` is a Python script that uses the `asyncio`, `json`, `psutil`, and `websockets` libraries to retrieve and send real-time statistics about the CPU, memory, and network usage of the computer it is running on.

## Dependencies

- Python 3.x
- `psutil`
- `websockets`

## Usage

1. Install the required dependencies by running `pip install psutil websockets`.
2. Run the script using `python pygetstatus.py`.
3. The script will start a WebSocket server on `localhost:8000` and send real-time statistics about the computer's CPU, memory, and network usage in JSON format.

# t3-app

`t3-app` is a React application that connects to the WebSocket server started by `pygetstatus.py` and displays real-time statistics about the computer's CPU, memory, and network usage using the `recharts` library.

## Dependencies

- Node.js
- `react`
- `recharts`

## Usage

1. Install the required dependencies by running `npm install`.
2. Start the application using `npm start`.
3. The application will connect to the WebSocket server started by `pygetstatus.py` and display real-time statistics about the computer's CPU, memory, and network usage.
