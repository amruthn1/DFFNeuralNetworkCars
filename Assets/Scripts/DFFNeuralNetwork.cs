using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace NeuralNetwork
{
    [System.Serializable]
    public class DFFNeuralNetwork : IComparable<DFFNeuralNetwork>
    {
        private int[] dimensions;
        //first [] symbolizes the input neuron and then the [,] symbolizes all of the weights corresponding for each hidden layer for that specific neuron and its path
        private float[][,] weights;
        private float[][] biases;
        //first one is the number of nodes in layer, second one is number of layers 
        private float[][] layers;
        private float fitness;

        public void NeuralNetwork(int[] nnDimensions)
        {
            this.dimensions = nnDimensions;
            this.fitness = 100;

            //Init neurons, weights, biases with random value between -1f and 1f
            initNeurons();
            initWeights();
            initBiases();
        }

        public void NeuralNetwork(int[] nnDimensions, float[][,] nnWeights, float[][] nnBiases, float[][] nnLayers, float nnFitness)
        {
            //Copy over values from passed in neural network values
            this.dimensions = nnDimensions;
            this.weights = nnWeights;
            this.biases = nnBiases;
            this.layers = nnLayers;
            this.fitness = nnFitness;
        }

        private void initNeurons()
        {
            //Generate all neurons and their corresponding values
            layers = new float[dimensions.Length][];
            for (int i = 0; i < dimensions.Length; i++)
            {
                //Create layers with neurons with specified sizes passed in from dimensions array
                layers[i] = new float[dimensions[i]];

                for (int j = 0; j < layers[i].Length; j++)
                {
                    //Assign random values between -1 and 1 for all neurons
                    layers[i][j] = UnityEngine.Random.Range(-1f, 1f);
                }
            }
        }

        private void initWeights()
        {
            //Generate weights for all the neurons for other layers
            weights = new float[dimensions.Length - 1][,];
            for (int k = 0; k < weights.Length; k++)
            {
                //Create random weights for all of the neurons after input layer
                weights[k] = new float[dimensions[k + 1], dimensions[k]];
                for (int l = 0; l < weights[k].GetLength(0); l++)
                {
                    for (int m = 0; m < weights[k].GetLength(1); m++)
                    {
                        //Assign random values between -1f and 1f to each weight
                        weights[k][l, m] = UnityEngine.Random.Range(-1f, 1f);
                    }
                }
            }
        }

        private void initBiases()
        {
            //Generate biases for all neurons for the other layers
            biases = new float[dimensions.Length - 1][];
            for (int n = 0; n < biases.Length; n++)
            {
                //Create random biases for all of the neurons after 
                biases[n] = new float[dimensions[n + 1]];
                for (int m = 0; m < biases[n].Length; m++)
                {
                    //Assign random values between -1f and 1f to each bias
                    biases[n][m] = UnityEngine.Random.Range(-1f, 1f);
                }
            }
        }

        public float[] FeedForward(float[] inputArr)
        {
            //Set input layer of neural network
            for (int i = 0; i < layers.Length; i++)
            {
                layers[0][i] = inputArr[i];
            }

            //Compute output layer 
            for (int j = 1; j < layers.Length; j++)
            {
                for (int l = 0; l < layers[j].Length; l++)
                {
                    //Coerce bias between -1f and 1f using Tanh activation function 
                    layers[j][l] = (float)Math.Tanh(addBias(j, l));
                }
            }
            //Return output layer
            return layers[layers.Length - 1];


        }

        private float addBias(int layer, int neuron)
        {
            //Declare initial value
            float sum = 0.0f;

            //Calculate weights of previous layer 
            for (int i = 0; i < layers[layer - 1].Length; i++)
            {
                //Multiply the value in the specific neuron by the weight for that specific neuron
                sum += layers[layer - 1][i] * weights[layer - 1][neuron, i];
            }
            //Add bias of current neuron to the sum
            sum += biases[layer - 1][neuron];

            //Return sum
            return sum;
        }

        public int CompareTo(DFFNeuralNetwork newNN)
        {
            throw new NotImplementedException();
        }
    }
}

