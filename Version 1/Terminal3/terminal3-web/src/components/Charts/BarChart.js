import React from 'react'
import {Bar} from 'react-chartjs-2'
import Loader from 'react-loader-spinner';

export const createLiveSample = (registerUsersNumber,GuestuserNumber,OwnerNumber,ManagerNumber,sysAdminNumber)=>{
    return {registerUsersNumber:registerUsersNumber,
        GuestuserNumber:GuestuserNumber,
        OwnerNumber:OwnerNumber,
        ManagerNumber:ManagerNumber,
        sysAdminNumber:sysAdminNumber};
}

const BarChart = ({sample})=> {

        const colors=[
            'rgba(255, 99, 132, 0.4)',
            'rgba(255, 159, 64, 0.4)',
            'rgba(255, 205, 86, 0.4)',
            'rgba(75, 192, 192, 0.4)',
            'rgba(54, 162, 235, 0.4)',
            'rgba(153, 102, 255, 0.4)',
            'rgba(201, 203, 207, 0.4)'
          ]
        if( Object.entries(sample).length === 0){return <div>
                <Loader type="ThreeDots" color="#FF5A5F" height="50" width="50" /></div>;}
        else{
            return <div> <Bar
            data={{
                labels: ['Live system view'],
                datasets:[{
                    label: 'Register Users',
                    data: [sample.registeredUsers],
                    backgroundColor: colors[0],
                        },
                    {
                    label: 'Guest Users',
                    data: [sample.guestUsers],
                    backgroundColor: colors[1],
                        },
                    {
                    label: 'Owners',
                    data: [sample.owners],
                    backgroundColor: colors[2],
                    },
                    {
                    label: 'Managers',
                    data: [sample.managersNotOwners],
                    backgroundColor: colors[3],
                    },
                    {
                    label: 'System Admins',
                    data: [sample.admins],
                    backgroundColor: colors[4],
                    }
                    ]

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

export default BarChart