import React, { useState, useContext, useEffect } from 'react';
import ModelContext from '../contex/ModelContext';
import DatabaseContext from '../contex/DatabaseContext';

const Add = () => {
  const { model, setModel, setUploaded } = useContext(ModelContext);
  const { database } = useContext(DatabaseContext)
  const  [formSubmitted, setFormSubmitted] = useState(false)

  const [art, setArt] = useState({
    'id':'',
    'title': '',
    'author': '',
    'rating': "",
    'relased': ""
  });

  const handleChange = e => {
    const { name, value } = e.target;
    if (name === 'rating' && (1 > value || value > 10)) {
      alert('Ocena musi zawierać się w przedziale 1-10');
      return;
    }

    if (( name === 'author') && value.length > 254) {
      alert("Przekroczono maksymalną liczbę znaków dla autora (254)");
      return;
    }
    
    if (( name === 'title') && value.length > 510) {
      alert("Przekroczono maksymalną liczbę znaków dla tytułu (510)");
      return;
    } 
    
    setArt(prevState => ({
      ...prevState,
      [name]: value
    }));
  };

  const upload = async (e) => {
    e.preventDefault()
    try {
      const response = await fetch(`${model}/${database}`, {
        method: 'POST',
        body: JSON.stringify(art),
        headers: {
          'Content-Type': 'application/json'
        }
      });
      if (!response.ok) {
        alert('Nie dodano elementu, sprawdz połączenie z bazą danych');
      } else {
        setUploaded(true)
        setFormSubmitted(true)
        // window.location.reload(false);
      }
    } catch(error) {
      console.error('Coś poszło nie tak: ' + error.message);
    }
  };

  useEffect(() => {
    if (formSubmitted) {
      setArt({
        'id': '',
        'title': '',
        'author': '',
        'rating': "",
        'relased': ""
      });
      setFormSubmitted(false);
    }
  }, [formSubmitted]);

  return (
    <div id='Add'>
      <hr/>
      <span>Dodaj nowy element: </span>
      <form onSubmit={(e) => upload(e)}>
        <label htmlFor='name'>Tytuł: </label>
        <input onChange={(e) => handleChange(e)} name='title' type='text' maxLength="510" id='name' value={art.title} required/>

        <label htmlFor='artist'>Twórca: </label>
        <input onChange={(e) => handleChange(e)} type='text' id='artist' maxLength="254" name='author' value={art.author} required/>

        <label htmlFor='relaseDate'>Data wydania: </label>
        <input onChange={(e) => handleChange(e)} name='relased' type='date' id='relaseDate' value={art.relased} required/>

        <label htmlFor='rating'>Ocena: </label>
        <input onChange={(e) => handleChange(e)} name='rating' type='number' min={'1'} max={'10'} value={art.rating} required/>

        <input type='submit' placeholder={'Dodaj do bazy danych'}/>
      </form>
      <hr/>
    </div>
  );
};

export default Add;