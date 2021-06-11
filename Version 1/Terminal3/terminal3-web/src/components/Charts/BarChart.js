import React from 'react'
import {Bar} from 'react-chartjs-2'

const BarChart = ({registerUsersNumber,GuestuserNumber,OwnerNumber,ManagerNumber,sysAdminNumber})=> {

        const colors=[
            'rgba(255, 99, 132, 0.4)',
            'rgba(255, 159, 64, 0.4)',
            'rgba(255, 205, 86, 0.4)',
            'rgba(75, 192, 192, 0.4)',
            'rgba(54, 162, 235, 0.4)',
            'rgba(153, 102, 255, 0.4)',
            'rgba(201, 203, 207, 0.4)'
          ]

        return <div> <Bar
                    data={{
                        labels: ['Live system view'],
                        datasets:[{
                            label: 'Register Users',
                            data: [registerUsersNumber],
                            backgroundColor: colors[0],
                                },
                            {
                            label: 'Guest Users',
                            data: [GuestuserNumber],
                            backgroundColor: colors[1],
                                },
                            {
                            label: 'Owners',
                            data: [OwnerNumber],
                            backgroundColor: colors[2],
                            },
                            {
                            label: 'Managers',
                            data: [ManagerNumber],
                            backgroundColor: colors[3],
                            },
                            {
                            label: 'System Admins',
                            data: [sysAdminNumber],
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

export default BarChart