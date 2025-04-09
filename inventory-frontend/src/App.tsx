import React, { useEffect, useState } from 'react';
import * as signalR from '@microsoft/signalr';
import './App.css';

interface InventoryItem {
  id: number;
  name: string;
  quantity: number;
  lastUpdated: string;
}

const App: React.FC = () => {
  const [inventory, setInventory] = useState<InventoryItem[]>([]);
  const [connection, setConnection] = useState<signalR.HubConnection | null>(null);

  useEffect(() => {
    const fetchInventory = async () => {
      try {
        const response = await fetch('https://localhost:5001/api/inventory');
        if (!response.ok) throw new Error(`HTTP error! status: ${response.status}`);
        const data = await response.json();
        setInventory(data);
      } catch (error) {
        console.error('Failed to fetch inventory:', error);
      }
    };
    fetchInventory();
  }, []);

  useEffect(() => {
    const newConnection = new signalR.HubConnectionBuilder()
      .withUrl('https://localhost:5001/inventoryHub')
      .withAutomaticReconnect()
      .build();

    setConnection(newConnection);
  }, []);

  useEffect(() => {
    if (connection) {
      connection.start()
        .then(() => {
          console.log('Connected to SignalR');
          connection.on('ReceiveInventoryUpdate', (item: InventoryItem) => {
            setInventory(prev => {
              const updated = [...prev];
              const index = updated.findIndex(i => i.id === item.id);
              if (index !== -1) {
                updated[index] = item;
              } else {
                updated.push(item);
              }
              return updated;
            });
          });
        })
        .catch(err => console.error('SignalR Connection Error: ', err));
    }

    return () => {
      connection?.stop();
    };
  }, [connection]);

  return (
    <div className="App">
      <h1>Real-Time Inventory Management</h1>
      <table>
        <thead>
          <tr>
            <th>ID</th>
            <th>Name</th>
            <th>Quantity</th>
            <th>Last Updated</th>
          </tr>
        </thead>
        <tbody>
          {inventory.map(item => (
            <tr key={item.id}>
              <td>{item.id}</td>
              <td>{item.name}</td>
              <td>{item.quantity}</td>
              <td>{new Date(item.lastUpdated).toLocaleString()}</td>
            </tr>
          ))}
        </tbody>
      </table>
    </div>
  );
};

export default App;