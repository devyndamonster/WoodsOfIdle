using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AddressableAssets;

namespace WoodsOfIdle
{
    public interface IAddressableLoaderService
    {
        public IEnumerator LoadAssets(AssetReferenceCollection assetCollection, Action<AssetReferenceCollection> onLoaded);
        public IEnumerator LoadAssets<T>(IEnumerable<AssetReference> references, Action<IEnumerable<T>> onLoaded);
    }
}
