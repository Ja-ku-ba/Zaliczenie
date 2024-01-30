import React, { useEffect, useState } from 'react';

const YourComponent = () => {
    const [data, setData] = useState([]);

    useEffect(() => {
        const fetchData = async () => {
            try {
                const response = await fetch('/api/yourcontroller/getdata');
                if (!response.ok) {
                    throw new Error(`HTTP error! Status: ${response.status}`);
                }
                const result = await response.json();
                setData(result);
            } catch (error) {
                console.error("Error fetching data:", error.message);
            }
        };

        fetchData();
    }, []);

    // Renderuj komponent używając danych z 'data'
    return (
        <div>
            {/* Twój kod renderujący */}
        </div>
    );
};

export default YourComponent;
