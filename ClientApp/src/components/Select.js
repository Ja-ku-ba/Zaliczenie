import React, { useContext, useEffect, useState } from 'react';
import ModelContext from '../contex/ModelContext';

const Select = () => {
  const { model, setModel } = useContext(ModelContext);
  const [marked, setMarked] = useState([false, false]);

  const ifSelected = (e, idx) => {
    setModel(e.target.id);
    setMarked([idx === 0, idx === 1]);
  };

  return (
    <div>
      <hr />
      {model !== "" ? <span>Zmień model</span> : <span>Wybierz model</span>}

      <form>
        <label htmlFor='song'>Muzyka</label>
        <input
          id='song'
          onChange={(e) => ifSelected(e, 0)}
          name='art'
          type='radio'
          checked={marked[0]}
        />

        <label htmlFor='movie'>Film</label>
        <input
          id='movie'
          onChange={(e) => ifSelected(e, 1)}
          name='art'
          type='radio'
          checked={marked[1]}
        />
      </form>
    </div>
  );
};

export default Select;
