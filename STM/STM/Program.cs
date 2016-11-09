﻿using System;
using System.IO;
using System.Collections.Generic;
using System.Threading.Tasks;
using TransactionalAction = System.Action<STM.IStmTransaction>;

namespace STM
{

    class Program
    {

        static void Main(string[] args)
        {
            var variable_1 = new StmMemory<int>();
            var variable_2 = new StmMemory<int>();
            File.WriteAllText("Log.txt", string.Empty);
            List<Action> actions = new List<Action>()
            {
                new Action(() => {
                    Stm.Do(new TransactionalAction((IStmTransaction transaction) => {
                        variable_1.Set(2, transaction);
                        variable_2.Set(3, transaction);
                    }));
                }),
                new Action(() => {
                    Stm.Do(new TransactionalAction((IStmTransaction transaction) => {
                        variable_1.Set(3, transaction);
                        variable_2.Set(4, transaction);
                    }));
                }),
                new Action(() => {
                    Stm.Do(new TransactionalAction((IStmTransaction transaction) => {
                        variable_1.Get(transaction);
                        variable_2.Get(transaction);
                    }));
                }),
                /*new Action(() => {
                    Stm.Do(new TransactionalAction((IStmTransaction transaction) => {
                        List<Action> subActions = new List<Action>(); 
                        Action sub_trans_1 = new Action(() => {
                                Stm.Do(new TransactionalAction((IStmTransaction sub_transaction) => {
                                    sub_transaction.SetParentTransaction(transaction);
                                    variable_1.Set(2, sub_transaction);
                                    variable_2.Set(3, sub_transaction);
                            }));
                        });
                        Action sub_trans_2 = new Action(() => {
                                Stm.Do(new TransactionalAction((IStmTransaction sub_transaction) => {
                                    sub_transaction.SetParentTransaction(transaction);
                                    variable_1.Set(2, sub_transaction);
                                    variable_2.Set(3, sub_transaction);
                            }));
                        });
                        subActions.Add(sub_trans_1);
                        subActions.Add(sub_trans_2);
                        List<Task> sub_tasks = new List<Task>();
                        foreach(Action action in subActions)
                        {
                            sub_tasks.Add(Task.Run(action));
                        }
                        foreach(Task task in sub_tasks)
                        {
                            task.Wait();
                        }
                    }));
                }),*/
            };
            List<Task> tasks = new List<Task>();
            foreach(Action action in actions)
            {
                tasks.Add(Task.Run(action));
            }
            //System.Threading.Thread.Sleep(10000000);
            foreach(Task task in tasks)
            {
                task.Wait();
            }
        }

    }

}