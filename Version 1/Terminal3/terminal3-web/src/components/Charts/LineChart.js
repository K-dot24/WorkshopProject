import React from 'react'
import {Line} from 'react-chartjs-2'

export const createDataGroup = (name,Xs,Ys)=>{return {Name:name, Xs:Xs,Ys:Ys};}
const LineChart = ({DataGroups})=> {
    return <div> <Line
    data={{
        labels: DataGroups[0].Xs,
        datasets:DataGroups.map( (dg)=>{return {
            label: dg.Name,
            data: dg.Ys,
            fill: false,
            borderColor: 'rgb(75, 192, 192)',
            tension: 0.1
                  };
                })
      }}
    height={400}
    width={600}
    options={{
        maintainAspectRatio:false,

    }}
        />
         </div>
}

export default LineChart