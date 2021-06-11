import React from 'react'
import {Line} from 'react-chartjs-2'

export const createDataGroup = (name,Xs,Ys)=>{return {Name:name, Xs:Xs,Ys:Ys};}
const LineChart = ({DataGroups})=> {
    console.log(typeof DataGroups)
    if( Object.entries(DataGroups).length === 0){return <div></div>;}
    else{
        const colors=['rgb(178,34,34)','rgb(186,85,211)','rgb(144,238,144)','rgb(65,105,225)','rgb(255,160,122)']

        return <div> <Line
        data={{
            labels: DataGroups[0].Xs,
            datasets:DataGroups.map( (dg,index)=>{return {
                label: dg.Name,
                data: dg.Ys,
                fill: false,
                borderColor: colors[index],
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

}

export default LineChart