import React, { useEffect, useState, useContext } from 'react'
import { useNavigate } from 'react-router-dom'

import ModelContext from '../contex/ModelContext'

const Delete = () => {
    const { model } = useContext(ModelContext)
    const nav = useNavigate()
    var path = window.location.pathname.split("/")
    const [art, setArt] = useState({})

    useEffect(() => {
		try {
			const getData = async () => {
				const response = await fetch(`/${model}/${path[3]}`)
				const data = await response.json()
				setArt(data)
			}
			getData()
		} catch (error) {
			console.log("Błąd podczas pobierania instanicji modelu: ", error)
		}
	}, [])

    const deleteElement = async () => {
		try{
			const response = await fetch(`/${model}/${art.id}`, {
				method: "DELETE"
			})
			if (response.ok) {
				nav("/")
			} else {
				console.log("Coś poszło nie tak podczas łączności z serwerem")
			}
		}	catch(error) {
			console.error("Coś poszło nie tak: ", error)
		}
    }


	return (
		    <div>
		        <h1>Nie bedziesz w stanie przywrócić usuniętego elemntu.</h1>
		
		        <table>
		            <thead>
		              <tr>
		                  <th>Tytuł:</th>
		                  <th>Twórca:</th>
		                  <th>Data wydania:</th> 
		                  <th>Ocena</th>
		              </tr>
		            </thead>
		              <tbody>
		                { art.length === 0 ?
							<tr className='tableInfo'>
								<td>Ładowanie...</td>
							</tr> :
		                    <tr>
		                      <td>{art.title}</td>
		                      <td>{art.author}</td>
		                      <td>{art.relased}</td>
		                      <td>{art.rating}</td>
		                    </tr> 
		                }
		              </tbody>
		          </table>
		
		        <label htmlFor='confirmDelete'>Potwierdź usunięcie</label>
		        <button id='confrmDelete' onClick={() => deleteElement()}>Potwierdzam, usuń</button>
		    </div>
		  )
}

export default Delete