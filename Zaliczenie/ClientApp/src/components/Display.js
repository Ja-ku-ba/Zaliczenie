import React, { useEffect, useState, useContext } from 'react'
import { Link } from 'react-router-dom'

import ModelContext from '../contex/ModelContext'

const Display = () => {
    const { model, uploaded, setUploaded } = useContext(ModelContext)
    const [data, setData] = useState("")
    const [responseStatus, setResponseStatus] = useState(false)

    useEffect(() => {
      setData("")
        const getData = async () => {
            try {
                const response = await fetch(`${model}`)
                if (!response.ok) {
                    throw new Error(`HTTP error! Status: ${response.status}`);
                }
                const result = await response.json();
                setData(result);
          } catch (error) {
            console.error("Error fetching data:", error.message);
          }
          setResponseStatus(true)
          setUploaded(false)
        };
    
        getData();
      }, [model, uploaded]);

	return (
    <div>
      <hr/>
        <table>
            <thead>

			<tr>
				<th>Tytuł:</th>
				<th>Twórca:</th>
				<th>Data wydania:</th>
				<th>Ocena</th>
				<th>Edytuj</th>
				<th>Usuń</th>
			</tr>
            </thead>
              <tbody>
                { data.length === 0 ? responseStatus ? <tr><td><span>Baza danych nie zawiera żadnych zapisów</span></td></tr> : <tr className='tableInfo'><td>Ładowanie...</td></tr> : 
					data.map(artData => 
						<tr key={artData.id}>
							<td>{artData.title}</td>
							<td>{artData.author}</td>
							<td>{artData.relased}</td>
							<td>{artData.rating}</td>
							<td><Link to={`/edit/${model}/${artData.id}`}>Edytuj</Link></td>
							<td><Link to={`/delete/${model}/${artData.id}`}>Usuń</Link></td>
						</tr>
                  	)
				}
              </tbody>
          </table>
    </div>
  )
}

export default Display