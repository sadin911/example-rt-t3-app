import { useEffect, useState } from 'react';

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

  return (
    <div>
      <h1>PC Status</h1>
      {pcStats ? (
        <table>
          <tbody>
            {Object.entries(pcStats).map(([key, value]) => (
              <tr key={key}>
                <td>{key}</td>
                <td>{String(value)}</td> {/* Convert value to string */}
              </tr>
            ))}
          </tbody>
        </table>
      ) : (
        <p>Loading PC status...</p>
      )}
    </div>
  );
};

export default PCStatus;
