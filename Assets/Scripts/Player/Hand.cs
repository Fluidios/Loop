using System;
using System.Collections;
using System.Collections.Generic;
using System.Reflection;
using UnityEngine;

public class Hand : GameSystem
{
    [SerializeField] private int _capacity = 5;
    [SerializeField] private float _holdedCardsSpacing = 50;
    [SerializeField] private float _holdedCardsHidingDepth = 25;
    private List<LocationCard> _cards = new List<LocationCard>();
    public override bool AsyncInitialization => false;

    public void AddToHand(LocationCard card)
    {
        _cards.Add(card);
        if (_cards.Count > _capacity)
            DiscardCardAtIndex(0, false);
        var positions = CalculateCardPositions();
        for (int i = 0; i < positions.Length-1; i++)
        {
            _cards[i].ThrowTo(positions[i] + Vector3.down * _holdedCardsHidingDepth);
        }
        var target = positions[positions.Length - 1];
        card.ThrowTo(target, callback: () =>
        {
            card.ThrowTo(target + Vector3.down * _holdedCardsHidingDepth);
        });
    }
    public void DiscardCardImidiatelly(LocationCard card, bool alignCardsAfterRemove)
    {
        _cards.Remove(card);
        if (alignCardsAfterRemove)
        {
            var positions = CalculateCardPositions();
            for (int i = 0; i < positions.Length; i++)
            {
                _cards[i].ThrowTo(positions[i] + Vector3.down * _holdedCardsHidingDepth);
            }
        }
        Destroy(card.gameObject);
    }
    public void DiscardCardAtIndex(int index, bool alignCardsAfterRemove)
    {
        var card = _cards[index];
        _cards.RemoveAt(index);
        card.ThrowTo(card.transform.position + Vector3.down * 10 * _holdedCardsHidingDepth, card.MoveSpeed, () =>
        {
            if (alignCardsAfterRemove)
            {
                var positions = CalculateCardPositions();
                for (int i = 0; i < positions.Length; i++)
                {
                    _cards[i].ThrowTo(positions[i] + Vector3.down * _holdedCardsHidingDepth);
                }
            }
            Destroy(card.gameObject);
        });
    }
    private Vector3[] CalculateCardPositions(int amount = 0)
    {
        if (amount <= 0) amount = _cards.Count;
        Vector3[] positions = new Vector3[amount];
        int halfCardsCount = amount / 2;
        if(amount % 2 == 0)
        {
            for (int i = 0; i < halfCardsCount; i++)
            {
                positions[halfCardsCount + i] = transform.position + (i+0.5f) * Vector3.right * _holdedCardsSpacing;
                positions[halfCardsCount - i-1] = transform.position + (i+0.5f) * Vector3.left * _holdedCardsSpacing;
            }
        }
        else
        {
            positions[halfCardsCount] = transform.position;
            for (int i = 0; i < halfCardsCount; i++)
            {
                positions[halfCardsCount + i + 1] = transform.position + (i + 1) * Vector3.right * _holdedCardsSpacing;
                positions[halfCardsCount - i - 1] = transform.position + (i + 1) * Vector3.left * _holdedCardsSpacing;
            }
        }
        return positions;
    }
    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.red;
        foreach (var item in CalculateCardPositions(_capacity))
        {
            Gizmos.DrawWireSphere(item, ((_holdedCardsSpacing)/2));
        }
    }
}
