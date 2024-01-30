import React, { useState, createContext, useEffect } from 'react';

const DatabaseContext = createContext();
export default DatabaseContext;

export const DatabaseProvider = ({ children }) => {
  const [database, setDatabase] = useState('');

  useEffect(() => {
    const cookies = document.cookie;

    // Ten warunek do poprawy
    if (!cookies.includes("database=mongo") && !cookies.includes("database=mysql")) {
      document.cookie = `database=${database}; expires=Fri, 31 Dec 2100 23:59:59 GMT`;
    }

    if (database === "") {
      if (cookies.includes("database=mongo")) {
        setDatabase('mongo');
      } else if (cookies.includes("database=mysql")) {
        setDatabase('mysql');
      } else {
        setDatabase("");
      }
    }
  }, [database]);

  const contextValue = {
    database: database,
    setDatabase: setDatabase,
  };

  return (
    <DatabaseContext.Provider value={contextValue}>
      {children}
      <br/>{database}
    </DatabaseContext.Provider>
  );
};
