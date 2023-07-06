import asyncio
import json
import psutil
import websockets

async def send_pc_stats(websocket, path):
    try:
        while True:
            pc_stats = get_pc_stats()
            try:
                await websocket.send(json.dumps(pc_stats))
            except websockets.exceptions.ConnectionClosedError:
                break
            await asyncio.sleep(0.500)  # Update the values every 100 milliseconds
    finally:
        await websocket.close()

def get_pc_stats():
    # Retrieve CPU and Memory statistics using psutil
    cpu_percent = psutil.cpu_percent()
    memory_percent = psutil.virtual_memory().percent
    # Retrieve network statistics
    network_stats = psutil.net_io_counters()
    network_bytes_sent = network_stats.bytes_sent
    network_bytes_received = network_stats.bytes_recv

    # Create the PC stats dictionary
    pc_stats = {
        'CPU Percent': cpu_percent,
        'Memory Percent': memory_percent,
        'Network Bytes Sent': network_bytes_sent/(1024*1024*1024),
        'Network Bytes Received': network_bytes_received/(1024*1024*1024)
    }

    return pc_stats


start_server = websockets.serve(send_pc_stats, "localhost", 8000)

asyncio.get_event_loop().run_until_complete(start_server)
asyncio.get_event_loop().run_forever()
