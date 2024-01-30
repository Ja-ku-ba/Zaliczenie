import React, { useContext, useEffect, useState } from 'react';
import DatabaseContext from '../contex/DatabaseContext';

const SelectDatabase = () => {
  const { database, setDatabase } = useContext(DatabaseContext);
  const [marked, setMarked] = useState([false, false]);       // [song, model], false means that none is selected

  const ifSelected = (e, idx) => {
    setDatabase(e.target.id);
    setMarked([idx === 0, idx === 1]);
  };
  useEffect(() => {
    if (database === "mongo") {
      setMarked([true, false])
    } else if (database === "mysql") {
      setMarked([false, true])
    } else {
      setMarked([false, false])
    }
  }, [database])

  return (
    <div>
      <hr />
      {database !== "" ? <span>Zmień bazę danych</span> : <span>Wybierz bazę danych</span>}

      <form>
        <input
          id='mongo'
          onChange={(e) => ifSelected(e, 0)}
          name='db'
          type='radio'
          checked={marked[0]}
        />
        <label htmlFor='mongo'>Mongo</label>
        <br/>
        <input
          id='mysql'
          onChange={(e) => ifSelected(e, 1)}
          name='db'
          type='radio'
          checked={marked[1]}
        />
        <label htmlFor='mysql'>Mysql</label>
      </form>
    </div>
  );
};

export default SelectDatabase;
