import { useEffect, useState } from 'react';
import { BarChart, Bar, XAxis, YAxis, Tooltip } from 'recharts';

const PCStatus = () => {
  const [pcStats, setPCStats] = useState(null);

  useEffect(() => {
    const socket = new WebSocket('ws://localhost:8000');

    socket.onmessage = (event) => {
      const data = JSON.parse(event.data);
      setPCStats(data);
    };

    return () => {
      socket.close();
    };
  }, []);

  // Convert pcStats object to an array of objects for use with recharts
  const chartData = pcStats
    ? Object.entries(pcStats).map(([key, value]) => ({ name: key, value }))
    : [];

  return (
    <div>
      <h1>PC Status</h1>
      {pcStats ? (
        <BarChart width={1024} height={600} data={chartData}>
          <XAxis dataKey="name" />
          <YAxis domain={[0, 100]} />
          <Tooltip />
          <Bar dataKey="value" fill="#8884d8" />
        </BarChart>
      ) : (
        <p>Loading PC status...</p>
      )}
    </div>
  );
};

export default PCStatus;
